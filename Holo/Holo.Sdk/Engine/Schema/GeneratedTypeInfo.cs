using System.Reflection;

namespace Holo.Sdk.Engine.Schema;

/// <summary>
/// Contains metadata about a dynamically generated struct type.
/// </summary>
public sealed class GeneratedTypeInfo
{
    /// <summary>
    /// Gets or sets the generated struct type.
    /// </summary>
    public required Type GeneratedType { get; init; }

    /// <summary>
    /// Gets or sets the type name as defined in the schema.
    /// </summary>
    public required string TypeName { get; init; }

    /// <summary>
    /// Gets or sets the field information for all fields in the struct.
    /// </summary>
    public required FieldMetadata[] Fields { get; init; }

    /// <summary>
    /// Gets or sets the constructor for creating instances of this type.
    /// </summary>
    public required ConstructorInfo Constructor { get; init; }

    /// <summary>
    /// Gets or sets the type attributes (e.g., @name, @description).
    /// </summary>
    public required Dictionary<string, string[]> TypeAttributes { get; init; }
}

/// <summary>
/// Contains metadata about a field in a generated struct.
/// </summary>
public sealed class FieldMetadata
{
    /// <summary>
    /// Gets or sets the field name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the .NET type of this field.
    /// </summary>
    public required Type FieldType { get; init; }

    /// <summary>
    /// Gets or sets the field info for the backing field.
    /// </summary>
    public required FieldInfo BackingField { get; init; }

    /// <summary>
    /// Gets or sets the property info for the public property.
    /// </summary>
    public required PropertyInfo Property { get; init; }

    /// <summary>
    /// Gets or sets whether this field is the primary key.
    /// </summary>
    public required bool IsPrimaryKey { get; init; }

    /// <summary>
    /// Gets or sets whether this field is nullable.
    /// </summary>
    public required bool IsNullable { get; init; }

    /// <summary>
    /// Gets or sets whether this field has a unique constraint.
    /// </summary>
    public required bool IsUnique { get; init; }

    /// <summary>
    /// Gets or sets whether this field is sensitive (e.g., passwords).
    /// </summary>
    public required bool IsSensitive { get; init; }

    /// <summary>
    /// Gets or sets the default value expression (e.g., "auto_increment").
    /// </summary>
    public required string? DefaultValue { get; init; }

    /// <summary>
    /// Gets or sets the validation function for this field, if any.
    /// The function signature is Func&lt;T, bool&gt; where T is the field type.
    /// </summary>
    public required Delegate? ValidationDelegate { get; init; }

    /// <summary>
    /// Gets or sets the comment/description for this field.
    /// </summary>
    public required string? Comment { get; init; }
}
