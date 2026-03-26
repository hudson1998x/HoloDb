using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.Productions;
using Holo.Sdk.Engine.SyntaxTree;
using Xunit;

namespace Holo.Sdk.Engine.Tests
{
    /// <summary>
    /// Helper class for testing the parser of the Holo SDK engine.
    /// Provides methods to parse input strings into syntax trees and assert
    /// expected node types and values in unit tests.
    /// </summary>
    public static class ParserTestHelper
    {
        /// <summary>
        /// Parses the given input string into a <see cref="SyntaxNode"/>.
        /// </summary>
        /// <param name="input">The input query string to parse.</param>
        /// <returns>The root <see cref="SyntaxNode"/> of the parsed syntax tree.</returns>
        public static SyntaxNode Parse(string input)
        {
            var tokens = QueryLexer.Parse(input.AsSpan());
            return Parser.Parse(tokens, input.AsSpan());
        }

        /// <summary>
        /// Parses the input string specifically as a <see cref="QueryNode"/>.
        /// </summary>
        /// <param name="input">The query string to parse.</param>
        /// <returns>The parsed <see cref="QueryNode"/>.</returns>
        public static QueryNode ParseAsQuery(string input)
        {
            var result = Parse(input);
            Assert.IsType<QueryNode>(result);
            return (QueryNode)result;
        }

        /// <summary>
        /// Asserts that the given node is not null and matches the specified type <typeparamref name="T"/>.
        /// Does not return the node.
        /// </summary>
        /// <typeparam name="T">The expected type of the node.</typeparam>
        /// <param name="node">The syntax node to check.</param>
        public static void AssertNodeTypeVoid<T>(SyntaxNode? node) where T : SyntaxNode
        {
            Assert.NotNull(node);
            Assert.IsType<T>(node);
        }

        /// <summary>
        /// Asserts that the given node is not null and of the specified type <typeparamref name="T"/>.
        /// Returns the node casted to the expected type.
        /// </summary>
        /// <typeparam name="T">The expected node type.</typeparam>
        /// <param name="node">The syntax node to check.</param>
        /// <returns>The node casted to <typeparamref name="T"/>.</returns>
        public static T AssertNodeType<T>(SyntaxNode? node) where T : SyntaxNode
        {
            Assert.NotNull(node);
            Assert.IsType<T>(node);
            return (T)node;
        }

        /// <summary>
        /// Asserts that a node is a <see cref="NamedBlockNode"/> with the expected name.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedName">The expected name of the block.</param>
        /// <returns>The <see cref="NamedBlockNode"/> instance.</returns>
        public static NamedBlockNode AssertNamedBlock(SyntaxNode? node, string expectedName)
        {
            var block = AssertNodeType<NamedBlockNode>(node);
            Assert.Equal(expectedName, TokenToString(block.Name.Value));
            return block;
        }

        /// <summary>
        /// Asserts that a node is an <see cref="IdentifierNode"/> with the expected value.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedValue">The expected identifier value.</param>
        /// <returns>The <see cref="IdentifierNode"/> instance.</returns>
        public static IdentifierNode AssertIdentifier(SyntaxNode? node, string expectedValue)
        {
            var id = AssertNodeType<IdentifierNode>(node);
            Assert.Equal(expectedValue, TokenToString(id.Value));
            return id;
        }

        /// <summary>
        /// Asserts that a node is a <see cref="LiteralNode"/> of the expected <see cref="TokenKind"/>.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedKind">The expected literal kind.</param>
        /// <returns>The <see cref="LiteralNode"/> instance.</returns>
        public static LiteralNode AssertLiteral(SyntaxNode? node, TokenKind expectedKind)
        {
            var literal = AssertNodeType<LiteralNode>(node);
            Assert.Equal(expectedKind, literal.Value.Kind);
            return literal;
        }

        /// <summary>
        /// Asserts that a node is a <see cref="FunctionAssignmentNode"/> with the expected field name.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedField">The expected field name of the function assignment.</param>
        /// <returns>The <see cref="FunctionAssignmentNode"/> instance.</returns>
        public static FunctionAssignmentNode AssertFunctionAssignment(SyntaxNode? node, string expectedField)
        {
            var assignment = AssertNodeType<FunctionAssignmentNode>(node);
            Assert.Equal(expectedField, TokenToString(assignment.Field.Value));
            return assignment;
        }

        /// <summary>
        /// Asserts that a node is an <see cref="AliasedNode"/> with the expected alias.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedAlias">The expected alias string.</param>
        /// <returns>The <see cref="AliasedNode"/> instance.</returns>
        public static AliasedNode AssertAliased(SyntaxNode? node, string expectedAlias)
        {
            var aliased = AssertNodeType<AliasedNode>(node);
            Assert.Equal(expectedAlias, TokenToString(aliased.Alias.Value));
            return aliased;
        }

        /// <summary>
        /// Asserts that a node is a <see cref="FilterFieldNode"/> with the expected field name.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <param name="expectedField">The expected field name.</param>
        /// <returns>The <see cref="FilterFieldNode"/> instance.</returns>
        public static FilterFieldNode AssertFilterField(SyntaxNode? node, string expectedField)
        {
            var filterField = AssertNodeType<FilterFieldNode>(node);
            Assert.Equal(expectedField, TokenToString(filterField.Field.Value));
            return filterField;
        }

        /// <summary>
        /// Asserts that a node is a <see cref="WhereClauseNode"/>.
        /// </summary>
        /// <param name="node">The syntax node to check.</param>
        /// <returns>The <see cref="WhereClauseNode"/> instance.</returns>
        public static WhereClauseNode AssertWhereClause(SyntaxNode? node)
        {
            return AssertNodeType<WhereClauseNode>(node);
        }

        /// <summary>
        /// Asserts that a <see cref="QueryNode"/> has the expected number of children.
        /// </summary>
        /// <param name="query">The <see cref="QueryNode"/> to check.</param>
        /// <param name="expectedCount">The expected number of child nodes.</param>
        public static void AssertChildCount(QueryNode query, int expectedCount)
        {
            Assert.Equal(expectedCount, query.Children.Count);
        }

        /// <summary>
        /// Gets the child node at the specified index from a <see cref="QueryNode"/>.
        /// </summary>
        /// <param name="query">The <see cref="QueryNode"/> to access.</param>
        /// <param name="index">The index of the child node.</param>
        /// <returns>The child <see cref="SyntaxNode"/> at the specified index.</returns>
        public static SyntaxNode? GetChild(QueryNode query, int index)
        {
            Assert.True(index < query.Children.Count, $"Index {index} out of range. Children count: {query.Children.Count}");
            return query.Children[index];
        }

        /// <summary>
        /// Gets the field items from a <see cref="NamedBlockNode"/>.
        /// </summary>
        /// <param name="block">The <see cref="NamedBlockNode"/>.</param>
        /// <returns>A list of field <see cref="SyntaxNode"/> items.</returns>
        public static List<SyntaxNode> GetFieldItems(NamedBlockNode block)
        {
            return block.Fields.Nodes;
        }

        /// <summary>
        /// Gets the argument items from a <see cref="FunctionCallNode"/>.
        /// </summary>
        /// <param name="funcCall">The function call node.</param>
        /// <returns>A list of argument <see cref="SyntaxNode"/> items.</returns>
        public static List<SyntaxNode> GetArgumentItems(FunctionCallNode funcCall)
        {
            return funcCall.Arguments.Nodes;
        }

        /// <summary>
        /// Gets the filter rules from a <see cref="FilterListNode"/>.
        /// </summary>
        /// <param name="filterList">The filter list node.</param>
        /// <returns>A list of filter <see cref="SyntaxNode"/> rules.</returns>
        public static List<SyntaxNode> GetFilterRules(FilterListNode filterList)
        {
            return filterList.Rules.Nodes;
        }

        /// <summary>
        /// Gets the filters from a <see cref="WhereClauseNode"/>.
        /// </summary>
        /// <param name="whereClause">The where clause node.</param>
        /// <returns>A list of filter <see cref="SyntaxNode"/> items.</returns>
        public static List<SyntaxNode> GetWhereClauseFilters(WhereClauseNode whereClause)
        {
            return whereClause.Filters.Nodes;
        }

        /// <summary>
        /// Gets the items from an <see cref="ArrayLiteralNode"/>.
        /// </summary>
        /// <param name="array">The array literal node.</param>
        /// <returns>A list of <see cref="SyntaxNode"/> items in the array.</returns>
        public static List<SyntaxNode> GetArrayItems(ArrayLiteralNode array)
        {
            return array.Items.Nodes;
        }

        /// <summary>
        /// Converts a <see cref="Token"/> to its string representation based on its kind.
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <returns>The string representation of the token kind.</returns>
        private static string TokenToString(Token token)
        {
            return token.Kind.ToString();
        }
    }
}