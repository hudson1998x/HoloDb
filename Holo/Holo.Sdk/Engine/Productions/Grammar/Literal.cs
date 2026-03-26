using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a literal value, which can be a string, number, boolean, or null.
    /// Examples: <c>'hello'</c>, <c>42</c>, <c>true</c>, <c>null</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="LiteralNode"/>
    /// containing the matched <see cref="Token"/> as its value.
    /// </returns>
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