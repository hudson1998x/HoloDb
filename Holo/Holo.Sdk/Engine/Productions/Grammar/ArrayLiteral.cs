using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production ArrayLiteral()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.LeftSquareBracket, _ => new EmptyNode()),
            
                Production.DelimitedList(
                    Production.Lazy(() => Literal()),
                    TokenKind.Comma,
                    nodes => new NodeList(nodes)
                ).As("items"),
            
                Production.TokenIs(TokenKind.RightSquareBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new ArrayLiteralNode
                {
                    Items = (NodeList)captured["items"]
                };
            }
        );
    }
}