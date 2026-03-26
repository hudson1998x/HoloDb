using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;
using Holo.Sdk.Engine.Tests;

namespace Holo.Tests.Engine.Productions
{
    /// <summary>
    /// Unit tests for the <see cref="Parser"/> in the Holo SDK engine.
    /// Tests cover parsing of queries, named blocks, function calls, aliases,
    /// literals, filters, and error handling.
    /// </summary>
    public class ParserTests
    {
        // ==================== Basic Parsing ====================

        /// <summary>
        /// Ensures that parsing an empty string returns a non-null syntax node.
        /// </summary>
        [Fact]
        public void Parses_Empty_Query()
        {
            var node = ParserTestHelper.Parse("");
            Assert.NotNull(node);
        }

        /// <summary>
        /// Verifies parsing of a simple named block with multiple identifier fields.
        /// </summary>
        [Fact]
        public void Parses_Simple_NamedBlock()
        {
            var query = ParserTestHelper.ParseAsQuery("User { id, name }");
            
            ParserTestHelper.AssertChildCount(query, 1);
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            Assert.Equal(TokenKind.Identifier, block.Name.Value.Kind);
            
            var fields = ParserTestHelper.GetFieldItems(block);
            Assert.Equal(2, fields.Count);
            ParserTestHelper.AssertNodeTypeVoid<IdentifierNode>(fields[0]);
            ParserTestHelper.AssertNodeTypeVoid<IdentifierNode>(fields[1]);
        }

        /// <summary>
        /// Verifies parsing of nested named blocks (blocks within blocks).
        /// </summary>
        [Fact]
        public void Parses_Nested_NamedBlock()
        {
            var query = ParserTestHelper.ParseAsQuery("User { creatorUser { id, name } }");
            
            var userBlock = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            Assert.Equal(TokenKind.Identifier, userBlock.Name.Value.Kind);
            
            var userFields = ParserTestHelper.GetFieldItems(userBlock);
            Assert.Single(userFields);
            
            var creatorBlock = ParserTestHelper.AssertNodeType<NamedBlockNode>(userFields[0]);
            Assert.Equal(TokenKind.Identifier, creatorBlock.Name.Value.Kind);
            
            var creatorFields = ParserTestHelper.GetFieldItems(creatorBlock);
            Assert.Equal(2, creatorFields.Count);
        }

        // ==================== Identifier Fields ====================

        /// <summary>
        /// Tests parsing of single identifier fields with various valid names.
        /// </summary>
        /// <param name="fieldName">The field name to parse.</param>
        [Theory]
        [InlineData("id")]
        [InlineData("user_name")]
        [InlineData("_privateField")]
        public void Parses_Identifier_Fields(string fieldName)
        {
            var query = ParserTestHelper.ParseAsQuery($"User {{ {fieldName} }}");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var field = ParserTestHelper.AssertNodeType<IdentifierNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, field.Value.Kind);
        }

        // ==================== Function Calls ====================

        /// <summary>
        /// Ensures parsing of a function call with a single argument.
        /// </summary>
        [Fact]
        public void Parses_FunctionCall_With_Args()
        {
            var query = ParserTestHelper.ParseAsQuery("User { count(id) }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var funcCall = ParserTestHelper.AssertNodeType<FunctionCallNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, funcCall.FunctionName.Value.Kind);
            
            var args = ParserTestHelper.GetArgumentItems(funcCall);
            Assert.Single(args);
            ParserTestHelper.AssertNodeTypeVoid<IdentifierNode>(args[0]);
        }

        /// <summary>
        /// Ensures parsing of a function call with multiple arguments.
        /// </summary>
        [Fact]
        public void Parses_FunctionCall_With_Multiple_Args()
        {
            var query = ParserTestHelper.ParseAsQuery("User { concat(firstName, lastName) }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var funcCall = ParserTestHelper.AssertNodeType<FunctionCallNode>(fields[0]);
            
            var args = ParserTestHelper.GetArgumentItems(funcCall);
            Assert.Equal(2, args.Count);
        }

        /// <summary>
        /// Ensures parsing of a function call whose argument is a named block.
        /// </summary>
        [Fact]
        public void Parses_FunctionCall_NamedBlock_Arg()
        {
            var query = ParserTestHelper.ParseAsQuery("User { subQuery(User { id }) }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var funcCall = ParserTestHelper.AssertNodeType<FunctionCallNode>(fields[0]);
            
            var args = ParserTestHelper.GetArgumentItems(funcCall);
            Assert.Single(args);
            ParserTestHelper.AssertNodeTypeVoid<NamedBlockNode>(args[0]);
        }

        // ==================== Aliased Fields ====================

        /// <summary>
        /// Ensures parsing of an aliased function call (e.g., count(id) as total).
        /// </summary>
        [Fact]
        public void Parses_Aliased_Function()
        {
            var query = ParserTestHelper.ParseAsQuery("User { count(id) as total }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var aliased = ParserTestHelper.AssertNodeType<AliasedNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, aliased.Alias.Value.Kind);
            
            var funcCall = ParserTestHelper.AssertNodeType<FunctionCallNode>(aliased.Left);
            Assert.Equal(TokenKind.Identifier, funcCall.FunctionName.Value.Kind);
        }

        /// <summary>
        /// Ensures parsing of an aliased identifier (e.g., name as n).
        /// </summary>
        [Fact]
        public void Parses_Aliased_Identifier()
        {
            var query = ParserTestHelper.ParseAsQuery("User { name as n }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var aliased = ParserTestHelper.AssertNodeType<AliasedNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, aliased.Alias.Value.Kind);
            
            ParserTestHelper.AssertNodeTypeVoid<IdentifierNode>(aliased.Left);
        }

        // ==================== Function Assignments ====================

        /// <summary>
        /// Ensures parsing of a function assignment with a named block argument.
        /// </summary>
        [Fact]
        public void Parses_FunctionAssignment_With_NamedBlock_Arg()
        {
            var query = ParserTestHelper.ParseAsQuery("userCount subQuery(User { id })");
            
            var assignment = ParserTestHelper.AssertNodeType<FunctionAssignmentNode>(ParserTestHelper.GetChild(query, 0));
            var funcCall = assignment.Function;
            Assert.Equal(TokenKind.Identifier, funcCall.FunctionName.Value.Kind);
            
            var args = ParserTestHelper.GetArgumentItems(funcCall);
            Assert.Single(args);
            var argBlock = ParserTestHelper.AssertNodeType<NamedBlockNode>(args[0]);
            var argFields = ParserTestHelper.GetFieldItems(argBlock);
            Assert.Single(argFields);
        }

        /// <summary>
        /// Ensures parsing of a function assignment with multiple arguments.
        /// </summary>
        [Fact]
        public void Parses_FunctionAssignment_With_Multiple_Args()
        {
            var query = ParserTestHelper.ParseAsQuery("result func(arg1, arg2)");
            
            var assignment = ParserTestHelper.AssertNodeType<FunctionAssignmentNode>(ParserTestHelper.GetChild(query, 0));
            var args = ParserTestHelper.GetArgumentItems(assignment.Function);
            Assert.Equal(2, args.Count);
        }

        /// <summary>
        /// Ensures parsing of a function assignment inside a named block.
        /// </summary>
        [Fact]
        public void Parses_FunctionAssignment_In_NamedBlock()
        {
            var query = ParserTestHelper.ParseAsQuery("User { userCount subQuery(User { id }) }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var assignment = ParserTestHelper.AssertNodeType<FunctionAssignmentNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, assignment.Field.Value.Kind);
        }

        // ==================== Filter Fields ====================

        /// <summary>
        /// Ensures parsing of a filter field containing multiple filter rules.
        /// </summary>
        [Fact]
        public void Parses_FilterField_FilterList()
        {
            var query = ParserTestHelper.ParseAsQuery("User { id { above 20, below 40 } }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var filterField = ParserTestHelper.AssertNodeType<FilterFieldNode>(fields[0]);
            Assert.Equal(TokenKind.Identifier, filterField.Field.Value.Kind);
            
            var filterList = ParserTestHelper.AssertNodeType<FilterListNode>(filterField.Filter);
            var rules = ParserTestHelper.GetFilterRules(filterList);
            Assert.Equal(2, rules.Count);
        }

        /// <summary>
        /// Ensures parsing of a filter field whose value is an array literal.
        /// </summary>
        [Fact]
        public void Parses_FilterField_With_Array_Value()
        {
            var query = ParserTestHelper.ParseAsQuery("User { status { in ['active', 'pending'] } }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var filterField = ParserTestHelper.AssertNodeType<FilterFieldNode>(fields[0]);
            
            var filterList = ParserTestHelper.AssertNodeType<FilterListNode>(filterField.Filter);
            var rules = ParserTestHelper.GetFilterRules(filterList);
            Assert.Single(rules);
            
            var rule = ParserTestHelper.AssertNodeType<FilterRuleNode>(rules[0]);
            Assert.Equal(TokenKind.KeywordIn, rule.Operator.Value.Kind);
            
            var array = ParserTestHelper.AssertNodeType<ArrayLiteralNode>(rule.Value);
            var items = ParserTestHelper.GetArrayItems(array);
            Assert.Equal(2, items.Count);
        }

        // ==================== Filter Binary ====================

        /// <summary>
        /// Ensures parsing of binary filter expressions (e.g., "above 10 and below 100").
        /// </summary>
        [Fact]
        public void Parses_FilterBinary_And()
        {
            var query = ParserTestHelper.ParseAsQuery("User { id { above 10 and below 100 } }");
            
            var block = ParserTestHelper.AssertNodeType<NamedBlockNode>(ParserTestHelper.GetChild(query, 0));
            var fields = ParserTestHelper.GetFieldItems(block);
            var filterField = ParserTestHelper.AssertNodeType<FilterFieldNode>(fields[0]);
            
            var binary = ParserTestHelper.AssertNodeType<BinaryExpressionNode>(filterField.Filter);
            Assert.Equal(TokenKind.Identifier, binary.Operator.Value.Kind);
        }
    }
}