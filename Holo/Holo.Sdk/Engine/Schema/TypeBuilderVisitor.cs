using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Schema;

/// <summary>
/// Visitor that generates readonly struct types from type definition nodes in the AST.
/// Uses System.Reflection.Emit to dynamically create types at runtime.
/// </summary>
public sealed class TypeBuilderVisitor : Visitor
{
    private static readonly ModuleBuilder s_moduleBuilder;
    private static int s_typeCounter;
    private static readonly Dictionary<string, Type> s_typeMap = new()
    {
        ["int"] = typeof(int),
        ["string"] = typeof(string),
        ["bool"] = typeof(bool),
        ["long"] = typeof(long),
        ["double"] = typeof(double),
        ["float"] = typeof(float),
        ["decimal"] = typeof(decimal),
        ["DateTime"] = typeof(DateTime),
        ["Guid"] = typeof(Guid)
    };

    private readonly Dictionary<string, GeneratedTypeInfo> _generatedTypes = new();
    private string? _currentTypeName;
    private string? _currentAttributeName;
    private readonly string _source;

    static TypeBuilderVisitor()
    {
        var assemblyName = new AssemblyName("Holo.Schema.Generated");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            assemblyName,
            AssemblyBuilderAccess.Run);
        s_moduleBuilder = assemblyBuilder.DefineDynamicModule("Holo.Schema.Generated");
    }

    public TypeBuilderVisitor(ReadOnlySpan<char> source)
    {
        _source = source.ToString();
    }

    /// <summary>
    /// Gets all generated types indexed by their type name.
    /// </summary>
    public IReadOnlyDictionary<string, GeneratedTypeInfo> GeneratedTypes => _generatedTypes;

    public override void VisitTypeDefinitionNode(TypeDefinitionNode node)
    {
        _currentTypeName = node.TypeName.Value.GetText(_source).ToString();
        
        // Collect field metadata
        var fields = new List<FieldMetadata>();
        var typeAttributes = new Dictionary<string, string[]>();

        // Visit attributes to collect type-level metadata
        foreach (var attr in node.Attributes.Nodes)
        {
            if (attr is AttributeNode attributeNode)
            {
                VisitAttributeNode(attributeNode);
                
                var attrName = attributeNode.AttributeName.Value.GetText(_source).ToString();
                var args = new List<string>();
                foreach (var arg in attributeNode.Arguments.Nodes)
                {
                    if (arg is IdentifierNode id)
                        args.Add(id.Value.GetText(_source).ToString());
                    else if (arg is LiteralNode lit)
                        args.Add(lit.Value.GetText(_source).ToString());
                }
                typeAttributes[attrName] = [.. args];
            }
        }

        // Visit field definitions
        foreach (var field in node.Fields.Nodes)
        {
            if (field is FieldDefinitionNode fieldDef)
            {
                var metadata = ExtractFieldMetadata(fieldDef);
                fields.Add(metadata);
            }
        }

        // Generate the struct type
        var generatedType = GenerateStructType(_currentTypeName, fields, typeAttributes);

        _currentTypeName = null;
    }

    public override void VisitAttributeNode(AttributeNode node)
    {
        _currentAttributeName = node.AttributeName.Value.GetText(_source).ToString();
        base.VisitAttributeNode(node);
        _currentAttributeName = null;
    }

    private FieldMetadata ExtractFieldMetadata(FieldDefinitionNode node)
    {
        var fieldName = node.FieldName.Value.GetText(_source).ToString();
        Type fieldType = typeof(object);
        bool isPrimaryKey = false;
        bool isNullable = false;
        bool isUnique = false;
        bool isSensitive = false;
        string? defaultValue = null;
        string? comment = null;
        Delegate? validationDelegate = null;

        foreach (var prop in node.Properties.Nodes)
        {
            if (prop is FieldPropertyNode property)
            {
                var propName = property.PropertyName.Value.GetText(_source).ToString();

                switch (propName)
                {
                    case "type":
                        if (property.Arguments.Nodes.Count > 0 && property.Arguments.Nodes[0] is IdentifierNode typeArg)
                        {
                            var typeName = typeArg.Value.GetText(_source).ToString();
                            if (s_typeMap.TryGetValue(typeName, out var mappedType))
                                fieldType = mappedType;
                        }
                        break;

                    case "primary":
                        isPrimaryKey = true;
                        break;

                    case "nullable":
                    case "null":
                        if (property.Arguments.Nodes.Count > 0 && property.Arguments.Nodes[0] is LiteralNode nullArg)
                        {
                            var nullVal = nullArg.Value.GetText(_source).ToString();
                            isNullable = nullVal == "true";
                        }
                        break;

                    case "unique":
                        isUnique = true;
                        break;

                    case "sensitive":
                        isSensitive = true;
                        break;

                    case "default":
                        if (property.Arguments.Nodes.Count > 0)
                        {
                            if (property.Arguments.Nodes[0] is IdentifierNode defaultId)
                                defaultValue = defaultId.Value.GetText(_source).ToString();
                            else if (property.Arguments.Nodes[0] is LiteralNode defaultLit)
                                defaultValue = defaultLit.Value.GetText(_source).ToString();
                        }
                        break;

                    case "comment":
                        if (property.Arguments.Nodes.Count > 0 && property.Arguments.Nodes[0] is LiteralNode commentLit)
                        {
                            comment = commentLit.Value.GetText(_source).ToString();
                        }
                        break;

                    case "validate":
                        if (property.Arguments.Nodes.Count > 0 && property.Arguments.Nodes[0] is IdentifierNode validatorId)
                        {
                            var validatorName = validatorId.Value.GetText(_source).ToString();
                            validationDelegate = ValidatorRegistry.GetValidator(validatorName, fieldType);
                        }
                        break;
                }
            }
        }

        return new FieldMetadata
        {
            Name = fieldName,
            FieldType = fieldType,
            BackingField = null!, // Will be set after type creation
            Property = null!,     // Will be set after type creation
            IsPrimaryKey = isPrimaryKey,
            IsNullable = isNullable,
            IsUnique = isUnique,
            IsSensitive = isSensitive,
            DefaultValue = defaultValue,
            ValidationDelegate = validationDelegate,
            Comment = comment
        };
    }

    private Type GenerateStructType(
        string typeName,
        List<FieldMetadata> fields,
        Dictionary<string, string[]> typeAttributes)
    {
        // Generate unique type name to avoid conflicts in tests and multiple compilations
        var uniqueTypeName = $"{typeName}_{Interlocked.Increment(ref s_typeCounter)}";
        
        // Create the type builder for a sealed value type (struct)
        var typeBuilder = s_moduleBuilder.DefineType(
            $"Holo.Schema.Generated.{uniqueTypeName}",
            TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
            typeof(ValueType));

        // Define fields and properties
        var fieldInfos = new List<FieldMetadata>();

        foreach (var field in fields)
        {
            // Define the private readonly backing field
            var backingField = typeBuilder.DefineField(
                $"<{field.Name}>k__BackingField",
                field.FieldType,
                FieldAttributes.Private | FieldAttributes.InitOnly);

            // Define the public property
            var propertyBuilder = typeBuilder.DefineProperty(
                field.Name,
                PropertyAttributes.HasDefault,
                field.FieldType,
                null);

            // Define the getter
            var getter = typeBuilder.DefineMethod(
                $"get_{field.Name}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                field.FieldType,
                Type.EmptyTypes);

            var getterIL = getter.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, backingField);
            getterIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getter);

            // Update the field metadata with actual FieldInfo and PropertyInfo
            fieldInfos.Add(new FieldMetadata
            {
                Name = field.Name,
                FieldType = field.FieldType,
                BackingField = backingField,
                Property = propertyBuilder,
                IsPrimaryKey = field.IsPrimaryKey,
                IsNullable = field.IsNullable,
                IsUnique = field.IsUnique,
                IsSensitive = field.IsSensitive,
                DefaultValue = field.DefaultValue,
                ValidationDelegate = field.ValidationDelegate,
                Comment = field.Comment
            });
        }

        // Define the constructor that takes all fields as parameters
        var paramTypes = fields.Select(f => f.FieldType).ToArray();
        var constructor = typeBuilder.DefineConstructor(
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
            CallingConventions.Standard,
            paramTypes);

        var ctorIL = constructor.GetILGenerator();

        for (int i = 0; i < fieldInfos.Count; i++)
        {
            ctorIL.Emit(OpCodes.Ldarg_0);           // Load 'this'
            ctorIL.Emit(OpCodes.Ldarg, i + 1);      // Load parameter (arg0 is 'this')
            ctorIL.Emit(OpCodes.Stfld, fieldInfos[i].BackingField);
        }

        ctorIL.Emit(OpCodes.Ret);

        // Create the type
        var createdType = typeBuilder.CreateType();

        // Get the actual FieldInfo and PropertyInfo from the created type
        var finalFields = fieldInfos.Select(f => new FieldMetadata
        {
            Name = f.Name,
            FieldType = f.FieldType,
            BackingField = createdType.GetField(f.BackingField.Name, BindingFlags.NonPublic | BindingFlags.Instance)!,
            Property = createdType.GetProperty(f.Property.Name, BindingFlags.Public | BindingFlags.Instance)!,
            IsPrimaryKey = f.IsPrimaryKey,
            IsNullable = f.IsNullable,
            IsUnique = f.IsUnique,
            IsSensitive = f.IsSensitive,
            DefaultValue = f.DefaultValue,
            ValidationDelegate = f.ValidationDelegate,
            Comment = f.Comment
        }).ToArray();

        var typeInfo = new GeneratedTypeInfo
        {
            GeneratedType = createdType,
            TypeName = typeName,
            Fields = finalFields,
            Constructor = createdType.GetConstructors()[0],
            TypeAttributes = typeAttributes
        };

        _generatedTypes[typeName] = typeInfo;

        return createdType;
    }
}
