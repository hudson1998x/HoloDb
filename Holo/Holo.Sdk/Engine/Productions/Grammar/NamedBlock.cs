using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production NamedBlock()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode
                    {
                        Value = t
                    })
                    .As("name"),

                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),

                Production.Lazy(() => Fields()).As("fields"),

                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new NamedBlockNode
                {
                    Name = (IdentifierNode)captured["name"],
                    Fields = (NodeList) captured["fields"]
                };
            }
        );
    }
}