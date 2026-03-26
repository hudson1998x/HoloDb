using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FunctionCall()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("name"),

                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                Production.DelimitedList(
                    Production.Choice(
                        Production.Lazy(() => NamedBlock()),
                        Production.Lazy(() => Field()) // recursion: arguments can be expressions
                    ),
                    TokenKind.Comma,
                    nodes =>
                    {
                        var args = new NodeList(nodes);
                        return args;
                    }
                ).As("args"),

                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured =>
            {
                return new FunctionCallNode()
                {
                    FunctionName = (IdentifierNode) captured["name"],
                    Arguments = (NodeList) captured["args"]
                };
            }
        );
    }
}