using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterGroup()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),
                Production.Lazy(() => FilterExpression()).As("expr"),
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured => captured["expr"]  // Just unwrap the group
        );
    }
}