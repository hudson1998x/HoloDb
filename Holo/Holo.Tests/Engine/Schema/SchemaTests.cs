using Holo.Sdk.Engine.Schema;

namespace Holo.Tests.Engine.Schema
{
    /// <summary>
    /// Unit tests for the schema-related functionality in the Holo SDK engine.
    /// Tests cover the <see cref="TypeBuilderVisitor"/>, <see cref="SchemaRegistry"/>,
    /// and <see cref="ValidatorRegistry"/> classes.
    /// 
    /// These tests must run sequentially due to shared static state in SchemaRegistry.
    /// </summary>
    [Collection("SchemaTests")]
    public class SchemaTests : IDisposable
    {
        /// <summary>
        /// Clears the schema registry before each test to ensure test isolation.
        /// </summary>
        public SchemaTests()
        {
            SchemaRegistry.Clear();
        }

        /// <summary>
        /// Cleans up after each test.
        /// </summary>
        public void Dispose()
        {
            SchemaRegistry.Clear();
        }

        #region TypeBuilderVisitor Tests

        /// <summary>
        /// Verifies that TypeBuilderVisitor can generate a simple struct type.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Generates_Simple_Struct()
        {
            var source = """
                type Person {
                    name {
                        type(string)
                    }
                    age {
                        type(int)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            Assert.Single(visitor.GeneratedTypes);
            Assert.True(visitor.GeneratedTypes.ContainsKey("Person"));

            var typeInfo = visitor.GeneratedTypes["Person"];
            Assert.Equal("Person", typeInfo.TypeName);
            Assert.Equal(2, typeInfo.Fields.Length);
            Assert.Equal("name", typeInfo.Fields[0].Name);
            Assert.Equal("age", typeInfo.Fields[1].Name);
        }

        /// <summary>
        /// Verifies that generated structs are value types.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Generates_ValueType()
        {
            var source = """
                type Item {
                    id {
                        type(int)
                        primary()
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["Item"];
            Assert.True(typeInfo.GeneratedType.IsValueType);
        }

        /// <summary>
        /// Verifies that field metadata is correctly captured including primary key flag.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Primary_Key()
        {
            var source = """
                type User {
                    id {
                        type(int)
                        primary()
                    }
                    name {
                        type(string)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            Assert.True(typeInfo.Fields[0].IsPrimaryKey);
            Assert.False(typeInfo.Fields[1].IsPrimaryKey);
        }

        /// <summary>
        /// Verifies that nullable field metadata is correctly captured.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Nullable_Fields()
        {
            var source = """
                type User {
                    id {
                        type(int)
                        null(false)
                    }
                    bio {
                        type(string)
                        null(true)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            Assert.False(typeInfo.Fields[0].IsNullable);
            Assert.True(typeInfo.Fields[1].IsNullable);
        }

        /// <summary>
        /// Verifies that unique field metadata is correctly captured.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Unique_Fields()
        {
            var source = """
                type User {
                    email {
                        type(string)
                        unique()
                    }
                    name {
                        type(string)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            Assert.True(typeInfo.Fields[0].IsUnique);
            Assert.False(typeInfo.Fields[1].IsUnique);
        }

        /// <summary>
        /// Verifies that sensitive field metadata is correctly captured.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Sensitive_Fields()
        {
            var source = """
                type User {
                    password {
                        type(string)
                        sensitive()
                    }
                    name {
                        type(string)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            Assert.True(typeInfo.Fields[0].IsSensitive);
            Assert.False(typeInfo.Fields[1].IsSensitive);
        }

        /// <summary>
        /// Verifies that type attributes are correctly captured.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Type_Attributes()
        {
            var source = """
                type User {
                    @name('UserEntity')
                    @description('A user in the system')
                    
                    id {
                        type(int)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            Assert.True(typeInfo.TypeAttributes.ContainsKey("name"));
            Assert.True(typeInfo.TypeAttributes.ContainsKey("description"));
            // Note: String literals include quotes in the token value
            Assert.Contains("'UserEntity'", typeInfo.TypeAttributes["name"]);
        }

        /// <summary>
        /// Verifies that generated types can be instantiated.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Generates_Instantiable_Type()
        {
            var source = """
                type Point {
                    x {
                        type(int)
                    }
                    y {
                        type(int)
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["Point"];
            var instance = typeInfo.Constructor.Invoke(new object[] { 10, 20 });

            Assert.NotNull(instance);
            Assert.True(instance.GetType().IsValueType);

            // Verify property values via reflection
            var xProp = typeInfo.Fields[0].Property;
            var yProp = typeInfo.Fields[1].Property;
            Assert.Equal(10, xProp.GetValue(instance));
            Assert.Equal(20, yProp.GetValue(instance));
        }

        /// <summary>
        /// Verifies that comment metadata is correctly captured.
        /// </summary>
        [Fact]
        public void TypeBuilderVisitor_Captures_Comments()
        {
            var source = """
                type User {
                    id {
                        type(int)
                        comment('The unique identifier')
                    }
                }
                """;

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            visitor.Visit(node);

            var typeInfo = visitor.GeneratedTypes["User"];
            // Note: String literals include quotes in the token value
            Assert.Equal("'The unique identifier'", typeInfo.Fields[0].Comment);
        }

        #endregion

        #region SchemaRegistry Tests

        /// <summary>
        /// Verifies that types can be registered and retrieved from the registry.
        /// </summary>
        [Fact]
        public void SchemaRegistry_Register_And_Retrieve()
        {
            var typeInfo = CreateTestTypeInfo("TestType");

            SchemaRegistry.Register(typeInfo);

            Assert.True(SchemaRegistry.TryGetType("TestType", out var retrieved));
            Assert.NotNull(retrieved);
            Assert.Equal("TestType", retrieved!.TypeName);
        }

        /// <summary>
        /// Verifies that TryGetType returns false for non-existent types.
        /// </summary>
        [Fact]
        public void SchemaRegistry_Returns_False_For_NonExistent_Type()
        {
            Assert.False(SchemaRegistry.TryGetType("NonExistent", out var result));
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that instances can be created via the registry.
        /// </summary>
        [Fact]
        public void SchemaRegistry_CreateInstance()
        {
            var typeInfo = CreateTestTypeInfo("Point");
            SchemaRegistry.Register(typeInfo);

            var instance = SchemaRegistry.CreateInstance("Point", 5, 10);

            Assert.NotNull(instance);
            // Note: Type name includes a counter suffix for uniqueness
            Assert.Contains("Holo.Schema.Generated.Point", instance.GetType().FullName);
        }

        /// <summary>
        /// Verifies that CreateInstance throws for non-existent types.
        /// </summary>
        [Fact]
        public void SchemaRegistry_CreateInstance_Throws_For_NonExistent()
        {
            Assert.Throws<InvalidOperationException>(() =>
                SchemaRegistry.CreateInstance("NonExistent", new object[] { }));
        }

        /// <summary>
        /// Verifies that types can be removed from the registry.
        /// </summary>
        [Fact]
        public void SchemaRegistry_Remove()
        {
            var typeInfo = CreateTestTypeInfo("ToRemove");
            SchemaRegistry.Register(typeInfo);

            Assert.True(SchemaRegistry.TryGetType("ToRemove", out _));

            var removed = SchemaRegistry.Remove("ToRemove");
            Assert.True(removed);
            Assert.False(SchemaRegistry.TryGetType("ToRemove", out _));
        }

        /// <summary>
        /// Verifies that the registry can be cleared.
        /// </summary>
        [Fact]
        public void SchemaRegistry_Clear()
        {
            // Ensure clean state
            SchemaRegistry.Clear();
            
            SchemaRegistry.Register(CreateTestTypeInfo("Type1"));
            SchemaRegistry.Register(CreateTestTypeInfo("Type2"));

            Assert.Equal(2, SchemaRegistry.Count);

            SchemaRegistry.Clear();

            Assert.Equal(0, SchemaRegistry.Count);
        }

        /// <summary>
        /// Verifies that GetAllTypeNames returns all registered type names.
        /// </summary>
        [Fact]
        public void SchemaRegistry_GetAllTypeNames()
        {
            // Ensure clean state
            SchemaRegistry.Clear();
            
            SchemaRegistry.Register(CreateTestTypeInfo("Alpha"));
            SchemaRegistry.Register(CreateTestTypeInfo("Beta"));

            var names = SchemaRegistry.GetAllTypeNames().ToList();

            Assert.Contains("Alpha", names);
            Assert.Contains("Beta", names);
            Assert.Equal(2, names.Count);
        }

        #endregion

        #region ValidatorRegistry Tests

        /// <summary>
        /// Verifies that the notEmpty validator works correctly for strings.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_NotEmpty_Validator()
        {
            var validator = ValidatorRegistry.GetValidator("notEmpty", typeof(string));
            Assert.NotNull(validator);

            var func = (Func<string, bool>)validator!;
            Assert.True(func("hello"));
            Assert.False(func(""));
            Assert.False(func("   "));
        }

        /// <summary>
        /// Verifies that the passwordValid validator works correctly.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_PasswordValid_Validator()
        {
            var validator = ValidatorRegistry.GetValidator("passwordValid", typeof(string));
            Assert.NotNull(validator);

            var func = (Func<string, bool>)validator!;
            Assert.True(func("Password1")); // Has upper, lower, digit, >= 8 chars
            Assert.False(func("pass")); // Too short
            Assert.False(func("password1")); // No uppercase
            Assert.False(func("PASSWORD1")); // No lowercase
            Assert.False(func("Password")); // No digit
        }

        /// <summary>
        /// Verifies that the emailValid validator works correctly.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_EmailValid_Validator()
        {
            var validator = ValidatorRegistry.GetValidator("emailValid", typeof(string));
            Assert.NotNull(validator);

            var func = (Func<string, bool>)validator!;
            Assert.True(func("test@example.com"));
            Assert.True(func("a@b.co"));
            Assert.False(func("invalid"));
            Assert.False(func("noatsign"));
        }

        /// <summary>
        /// Verifies that GetValidator returns null for non-existent validators.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_Returns_Null_For_NonExistent()
        {
            var validator = ValidatorRegistry.GetValidator("nonExistent", typeof(string));
            Assert.Null(validator);
        }

        /// <summary>
        /// Verifies that GetValidator returns null for type mismatch.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_Returns_Null_For_TypeMismatch()
        {
            // notEmpty is for strings, not ints
            var validator = ValidatorRegistry.GetValidator("notEmpty", typeof(int));
            Assert.Null(validator);
        }

        /// <summary>
        /// Verifies that validators can be registered.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_Register_Custom_Validator()
        {
            // Register a custom validator - wrapped in Func<object, bool> for storage
            ValidatorRegistry.Register("isPositive", typeof(int), val => (int)val > 0);

            // For custom validators, we can only verify they're stored
            Assert.True(ValidatorRegistry.Exists("isPositive"));
            Assert.True(ValidatorRegistry.GetAllValidatorNames().Contains("isPositive"));
        }

        /// <summary>
        /// Verifies that GetAllValidatorNames returns all registered validators.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_GetAllValidatorNames()
        {
            var names = ValidatorRegistry.GetAllValidatorNames().ToList();

            Assert.Contains("notEmpty", names);
            Assert.Contains("passwordValid", names);
            Assert.Contains("emailValid", names);
        }

        /// <summary>
        /// Verifies that Exists returns true for built-in validators.
        /// </summary>
        [Fact]
        public void ValidatorRegistry_Exists()
        {
            Assert.True(ValidatorRegistry.Exists("notEmpty"));
            Assert.True(ValidatorRegistry.Exists("passwordValid"));
            Assert.True(ValidatorRegistry.Exists("emailValid"));
            Assert.False(ValidatorRegistry.Exists("nonExistent"));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a test <see cref="GeneratedTypeInfo"/> for testing purposes.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>A <see cref="GeneratedTypeInfo"/> instance.</returns>
        private static GeneratedTypeInfo CreateTestTypeInfo(string typeName)
        {
            // We need to create a real dynamic type for the constructor
            var source = "type " + typeName + " {\n" +
                         "    x {\n" +
                         "        type(int)\n" +
                         "    }\n" +
                         "    y {\n" +
                         "        type(int)\n" +
                         "    }\n" +
                         "}";

            var tokens = Holo.Sdk.Engine.Lexer.QueryLexer.Parse(source.AsSpan());
            var node = Holo.Sdk.Engine.Productions.Parser.Parse(tokens, source.AsSpan());

            var visitor = new TypeBuilderVisitor(source.AsSpan());
            
            // Visit only the type definition directly to avoid registering to SchemaRegistry
            if (node is Holo.Sdk.Engine.SyntaxTree.QueryNode query)
            {
                foreach (var child in query.Children)
                {
                    if (child is Holo.Sdk.Engine.SyntaxTree.TypeDefinitionNode typeDef)
                    {
                        visitor.VisitTypeDefinitionNode(typeDef);
                    }
                }
            }

            return visitor.GeneratedTypes[typeName];
        }

        #endregion
    }
}
