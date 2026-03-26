using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;
using Holo.Sdk.Engine.Tests;
using Xunit;

namespace Holo.Tests.Engine.Productions
{
    /// <summary>
    /// Unit tests for the schema-related grammar productions in the Holo SDK engine.
    /// Tests cover parsing of type definitions, field definitions, field properties,
    /// and attributes.
    /// </summary>
    public class SchemaParserTests
    {
        #region TypeDefinition Tests

        /// <summary>
        /// Verifies parsing of a simple type definition with a single field.
        /// </summary>
        [Fact]
        public void Parses_Simple_TypeDefinition()
        {
            var source = """
                type User {
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            ParserTestHelper.AssertChildCount(query, 1);

            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            Assert.Single(typeDef.Fields.Nodes);
        }

        /// <summary>
        /// Verifies parsing of a type definition with multiple fields.
        /// </summary>
        [Fact]
        public void Parses_TypeDefinition_With_Multiple_Fields()
        {
            var source = """
                type Product {
                    id {
                        type(int)
                    }
                    name {
                        type(string)
                    }
                    price {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");

            Assert.Equal(3, typeDef.Fields.Nodes.Count);
        }

        /// <summary>
        /// Verifies parsing of a type definition with attributes.
        /// </summary>
        [Fact]
        public void Parses_TypeDefinition_With_Attributes()
        {
            var source = """
                type User {
                    @name('User')
                    @description('A user entity')
                    
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");

            Assert.Equal(2, typeDef.Attributes.Nodes.Count);
            Assert.Single(typeDef.Fields.Nodes);
        }

        /// <summary>
        /// Verifies parsing of a type definition with the @timestamps() attribute.
        /// </summary>
        [Fact]
        public void Parses_TypeDefinition_With_Timestamps_Attribute()
        {
            var source = """
                type User {
                    @timestamps()
                    
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");

            Assert.Single(typeDef.Attributes.Nodes);
            var attr = ParserTestHelper.AssertAttribute(typeDef.Attributes.Nodes[0], "Identifier");
            Assert.Empty(attr.Arguments.Nodes);
        }

        #endregion

        #region FieldDefinition Tests

        /// <summary>
        /// Verifies parsing of a field definition with a single property.
        /// </summary>
        [Fact]
        public void Parses_FieldDefinition_With_Single_Property()
        {
            var source = """
                type User {
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");

            Assert.Single(fieldDef.Properties.Nodes);
        }

        /// <summary>
        /// Verifies parsing of a field definition with multiple properties.
        /// </summary>
        [Fact]
        public void Parses_FieldDefinition_With_Multiple_Properties()
        {
            var source = """
                type User {
                    id {
                        type(int)
                        primary()
                        null(false)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");

            Assert.Equal(3, fieldDef.Properties.Nodes.Count);
        }

        #endregion

        #region FieldProperty Tests

        /// <summary>
        /// Verifies parsing of a field property with a single argument.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_With_Argument()
        {
            var source = """
                type User {
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordType");

            Assert.Single(prop.Arguments.Nodes);
            Assert.IsType<IdentifierNode>(prop.Arguments.Nodes[0]);
        }

        /// <summary>
        /// Verifies parsing of a field property with no arguments.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_Without_Arguments()
        {
            var source = """
                type User {
                    id {
                        primary()
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordPrimary");

            Assert.Empty(prop.Arguments.Nodes);
        }

        /// <summary>
        /// Verifies parsing of a field property with a string argument.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_With_String_Argument()
        {
            var source = """
                type User {
                    id {
                        comment('The user identifier')
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordComment");

            Assert.Single(prop.Arguments.Nodes);
            Assert.IsType<LiteralNode>(prop.Arguments.Nodes[0]);
        }

        /// <summary>
        /// Verifies parsing of the type property with int keyword.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_Type_Int()
        {
            var source = """
                type User {
                    count {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordType");

            var arg = Assert.IsType<IdentifierNode>(prop.Arguments.Nodes[0]);
            Assert.Equal(TokenKind.KeywordInt, arg.Value.Kind);
        }

        /// <summary>
        /// Verifies parsing of the type property with string keyword.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_Type_String()
        {
            var source = """
                type User {
                    name {
                        type(string)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordType");

            var arg = Assert.IsType<IdentifierNode>(prop.Arguments.Nodes[0]);
            Assert.Equal(TokenKind.KeywordString, arg.Value.Kind);
        }

        /// <summary>
        /// Verifies parsing of the default property with auto_increment.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_Default_AutoIncrement()
        {
            var source = """
                type User {
                    id {
                        default(auto_increment)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordDefault");

            var arg = Assert.IsType<IdentifierNode>(prop.Arguments.Nodes[0]);
            Assert.Equal(TokenKind.KeywordAutoIncrement, arg.Value.Kind);
        }

        /// <summary>
        /// Verifies parsing of the validate property.
        /// </summary>
        [Fact]
        public void Parses_FieldProperty_Validate()
        {
            var source = """
                type User {
                    email {
                        validate(emailValid)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var fieldDef = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            var prop = ParserTestHelper.AssertFieldProperty(fieldDef.Properties.Nodes[0], "KeywordValidate");

            Assert.Single(prop.Arguments.Nodes);
            var arg = Assert.IsType<IdentifierNode>(prop.Arguments.Nodes[0]);
            Assert.Equal(TokenKind.Identifier, arg.Value.Kind);
        }

        #endregion

        #region Attribute Tests

        /// <summary>
        /// Verifies parsing of an attribute with a single argument.
        /// </summary>
        [Fact]
        public void Parses_Attribute_With_Argument()
        {
            var source = """
                type User {
                    @name('User')
                    
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var attr = ParserTestHelper.AssertAttribute(typeDef.Attributes.Nodes[0], "Identifier");

            Assert.Single(attr.Arguments.Nodes);
            Assert.IsType<LiteralNode>(attr.Arguments.Nodes[0]);
        }

        /// <summary>
        /// Verifies parsing of an attribute with no arguments.
        /// </summary>
        [Fact]
        public void Parses_Attribute_Without_Arguments()
        {
            var source = """
                type User {
                    @timestamps()
                    
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            var attr = ParserTestHelper.AssertAttribute(typeDef.Attributes.Nodes[0], "Identifier");

            Assert.Empty(attr.Arguments.Nodes);
        }

        /// <summary>
        /// Verifies parsing of multiple attributes.
        /// </summary>
        [Fact]
        public void Parses_Multiple_Attributes()
        {
            var source = """
                type User {
                    @name('User')
                    @description('A user entity')
                    @timestamps()
                    
                    id {
                        type(int)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");

            Assert.Equal(3, typeDef.Attributes.Nodes.Count);
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Verifies parsing of the full schema example from Program.cs.
        /// </summary>
        [Fact]
        public void Parses_Full_Schema_Example()
        {
            var source = """
                type User {
                    @name('User')
                    @description('This is the user entity')
                    
                    id {
                        type(int)
                        default(auto_increment)
                        primary()
                        null(false)
                        comment('The user ID')
                    }
                    name {
                        type(string)
                        null(false)
                        comment('The users full name')
                    }
                    email {
                        type(string)
                        null(false)
                        unique()
                        validate(emailValid)
                        comment('The users email')
                    }
                    
                    @timestamps()
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            ParserTestHelper.AssertChildCount(query, 1);

            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");
            
            // Verify attributes
            Assert.Equal(3, typeDef.Attributes.Nodes.Count);
            
            // Verify fields
            Assert.Equal(3, typeDef.Fields.Nodes.Count);
            
            // Verify first field (id) has all properties
            var idField = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            Assert.Equal(5, idField.Properties.Nodes.Count);
        }

        /// <summary>
        /// Verifies that the parser correctly handles mixed field property types.
        /// </summary>
        [Fact]
        public void Parses_Mixed_Field_Properties()
        {
            var source = """
                type Product {
                    id {
                        type(int)
                        primary()
                    }
                    name {
                        type(string)
                        null(false)
                    }
                    tags {
                        type(string)
                        null(true)
                    }
                }
                """;

            var result = ParserTestHelper.Parse(source);
            var query = ParserTestHelper.AssertNodeType<QueryNode>(result);
            var typeDef = ParserTestHelper.AssertTypeDefinition(query.Children[0], "Identifier");

            Assert.Equal(3, typeDef.Fields.Nodes.Count);

            // First field: 2 properties (type, primary)
            var idField = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[0], "Identifier");
            Assert.Equal(2, idField.Properties.Nodes.Count);

            // Second field: 2 properties (type, null)
            var nameField = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[1], "Identifier");
            Assert.Equal(2, nameField.Properties.Nodes.Count);

            // Third field: 2 properties (type, null with true)
            var tagsField = ParserTestHelper.AssertFieldDefinition(typeDef.Fields.Nodes[2], "Identifier");
            Assert.Equal(2, tagsField.Properties.Nodes.Count);
        }

        #endregion
    }
}
