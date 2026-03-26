using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;
using Xunit;

namespace Holo.Tests.Engine.SyntaxTree
{
    /// <summary>
    /// Unit tests for the visitor pattern implementation in the Holo SDK engine.
    /// Tests cover the <see cref="IVisitor"/>, <see cref="IVisitor{TResult}"/>,
    /// <see cref="Visitor"/>, and <see cref="Visitor{TResult}"/> classes.
    /// </summary>
    public class VisitorTests
    {
        /// <summary>
        /// Test visitor that counts the number of nodes visited.
        /// </summary>
        private class CountingVisitor : Visitor
        {
            public int NodeCount { get; private set; }

            public override void Visit(SyntaxNode node)
            {
                NodeCount++;
                base.Visit(node);
            }

            public override void VisitEmptyNode(EmptyNode node)
            {
                NodeCount++;
            }

            public override void VisitQueryNode(QueryNode node)
            {
                NodeCount++;
                base.VisitQueryNode(node);
            }
        }

        /// <summary>
        /// Test visitor that collects visited node types.
        /// </summary>
        private class CollectingVisitor : Visitor
        {
            public List<string> VisitedTypes { get; } = new();

            public override void Visit(SyntaxNode node)
            {
                VisitedTypes.Add(node.GetType().Name);
                base.Visit(node);
            }
        }

        /// <summary>
        /// Verifies that the visitor correctly traverses a simple query node.
        /// </summary>
        [Fact]
        public void Visitor_Traverses_QueryNode()
        {
            var queryNode = new QueryNode();
            queryNode.Children.Add(new EmptyNode());
            queryNode.Children.Add(new EmptyNode());

            var visitor = new CountingVisitor();
            visitor.Visit(queryNode);

            // QueryNode (1) + QueryNode's VisitQueryNode (1) + 2 EmptyNodes (2) + 2 EmptyNode.VisitEmptyNode calls (2) = 6
            Assert.Equal(6, visitor.NodeCount);
        }

        /// <summary>
        /// Verifies that the visitor correctly traverses nested named blocks.
        /// </summary>
        [Fact]
        public void Visitor_Traverses_Nested_NamedBlock()
        {
            var innerBlock = new NamedBlockNode
            {
                Name = new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 0, EndPosition = 5 } },
                Fields = new NodeList()
            };

            var outerBlock = new NamedBlockNode
            {
                Name = new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 0, EndPosition = 5 } },
                Fields = new NodeList(new List<SyntaxNode> { innerBlock })
            };

            var visitor = new CollectingVisitor();
            visitor.Visit(outerBlock);

            Assert.Contains("NamedBlockNode", visitor.VisitedTypes);
            Assert.Contains("IdentifierNode", visitor.VisitedTypes);
            Assert.Contains("NodeList", visitor.VisitedTypes);
        }

        /// <summary>
        /// Verifies that the visitor correctly traverses type definition nodes.
        /// </summary>
        [Fact]
        public void Visitor_Traverses_TypeDefinitionNode()
        {
            var fieldDef = new FieldDefinitionNode
            {
                FieldName = new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 0, EndPosition = 2 } },
                Properties = new NodeList()
            };

            var typeDef = new TypeDefinitionNode
            {
                TypeName = new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 5, EndPosition = 9 } },
                Attributes = new NodeList(),
                Fields = new NodeList(new List<SyntaxNode> { fieldDef })
            };

            var visitor = new CollectingVisitor();
            visitor.Visit(typeDef);

            Assert.Contains("TypeDefinitionNode", visitor.VisitedTypes);
            Assert.Contains("FieldDefinitionNode", visitor.VisitedTypes);
        }

        /// <summary>
        /// Verifies that the visitor correctly traverses field property nodes.
        /// </summary>
        [Fact]
        public void Visitor_Traverses_FieldPropertyNode()
        {
            var prop = new FieldPropertyNode
            {
                PropertyName = new IdentifierNode { Value = new Token { Kind = TokenKind.KeywordType, StartPosition = 0, EndPosition = 4 } },
                Arguments = new NodeList(new List<SyntaxNode>
                {
                    new IdentifierNode { Value = new Token { Kind = TokenKind.KeywordInt, StartPosition = 5, EndPosition = 8 } }
                })
            };

            var visitor = new CollectingVisitor();
            visitor.Visit(prop);

            Assert.Contains("FieldPropertyNode", visitor.VisitedTypes);
            Assert.Contains("IdentifierNode", visitor.VisitedTypes);
            Assert.Contains("NodeList", visitor.VisitedTypes);
        }

        /// <summary>
        /// Verifies that the visitor correctly traverses attribute nodes.
        /// </summary>
        [Fact]
        public void Visitor_Traverses_AttributeNode()
        {
            var attr = new AttributeNode
            {
                AttributeName = new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 1, EndPosition = 5 } },
                Arguments = new NodeList(new List<SyntaxNode>
                {
                    new LiteralNode { Value = new Token { Kind = TokenKind.StringLiteral, StartPosition = 6, EndPosition = 12 } }
                })
            };

            var visitor = new CollectingVisitor();
            visitor.Visit(attr);

            Assert.Contains("AttributeNode", visitor.VisitedTypes);
            Assert.Contains("IdentifierNode", visitor.VisitedTypes);
            Assert.Contains("LiteralNode", visitor.VisitedTypes);
        }

        /// <summary>
        /// Verifies that the generic visitor returns correct values.
        /// </summary>
        [Fact]
        public void Generic_Visitor_Returns_Values()
        {
            var countingVisitor = new CountingGenericVisitor();
            var emptyNode = new EmptyNode();
            
            var result = countingVisitor.Visit(emptyNode);
            
            Assert.Equal(1, result);
        }

        /// <summary>
        /// Generic test visitor that returns the depth of traversal.
        /// </summary>
        private class CountingGenericVisitor : Visitor<int>
        {
            public override int VisitEmptyNode(EmptyNode node) => 1;
            public override int VisitQueryNode(QueryNode node) => 2;
        }

        /// <summary>
        /// Verifies that Accept method is called correctly on syntax nodes.
        /// </summary>
        [Fact]
        public void SyntaxNode_Accept_Calls_Visitor()
        {
            var visitor = new CountingVisitor();
            var node = new EmptyNode();
            
            node.Accept(visitor);
            
            Assert.Equal(1, visitor.NodeCount);
        }

        /// <summary>
        /// Verifies that the visitor can be used to collect specific node types.
        /// </summary>
        [Fact]
        public void Visitor_Collects_Specific_Node_Types()
        {
            var query = new QueryNode();
            query.Children.Add(new IdentifierNode { Value = new Token { Kind = TokenKind.Identifier, StartPosition = 0, EndPosition = 3 } });
            query.Children.Add(new LiteralNode { Value = new Token { Kind = TokenKind.NumberLiteral, StartPosition = 4, EndPosition = 5 } });
            query.Children.Add(new EmptyNode());

            var identifierCount = 0;
            var literalCount = 0;

            var countingVisitor = new SpecificTypeVisitor(
                () => identifierCount++,
                () => literalCount++);

            countingVisitor.Visit(query);

            Assert.Equal(1, identifierCount);
            Assert.Equal(1, literalCount);
        }

        /// <summary>
        /// Visitor that counts specific node types.
        /// </summary>
        private class SpecificTypeVisitor : Visitor
        {
            private readonly Action _onIdentifier;
            private readonly Action _onLiteral;

            public SpecificTypeVisitor(Action onIdentifier, Action onLiteral)
            {
                _onIdentifier = onIdentifier;
                _onLiteral = onLiteral;
            }

            public override void VisitIdentifierNode(IdentifierNode node)
            {
                _onIdentifier();
            }

            public override void VisitLiteralNode(LiteralNode node)
            {
                _onLiteral();
            }
        }
    }
}
