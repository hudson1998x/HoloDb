using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FunctionAssignment()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("target"),

                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("functionName"),

                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                Production.DelimitedList(
                    Production.Choice(
                        Production.Lazy(() => NamedBlock()),
                        Production.Lazy(() => Field())
                    ),
                    TokenKind.Comma,
                    nodes => new NodeList(nodes)
                ).As("args"),

                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured =>
            {
                return new FunctionAssignmentNode
                {
                    Field = (IdentifierNode)captured["target"],
                    Function = new FunctionCallNode()
                    {
                        FunctionName = (IdentifierNode)captured["functionName"],
                        Arguments = (NodeList)captured["args"]
                    }
                };
            }
        );
    }
}