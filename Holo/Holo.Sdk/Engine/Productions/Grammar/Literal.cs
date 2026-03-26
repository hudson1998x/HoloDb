using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production Literal()
    {
        return Production.Choice(
            Production.TokenIs(TokenKind.StringLiteral, t => new LiteralNode { Value = t }),
            Production.TokenIs(TokenKind.NumberLiteral, t => new LiteralNode { Value = t }),
            Production.TokenIs(TokenKind.BooleanLiteral, t => new LiteralNode { Value = t }),
            Production.TokenIs(TokenKind.NullLiteral, t => new LiteralNode { Value = t })
        );
    }
}