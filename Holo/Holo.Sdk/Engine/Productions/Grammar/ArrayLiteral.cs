using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Creates a production that parses an array literal.
    /// Matches a left square bracket, zero or more literals separated by commas, and a right square bracket.
    /// Example: <c>[1, 2, "text"]</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that parses an array literal and returns an <see cref="ArrayLiteralNode"/>.
    /// </returns>
    public static Production ArrayLiteral()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Match the opening bracket
                Production.TokenIs(TokenKind.LeftSquareBracket, _ => new EmptyNode()),
            
                // Match a delimited list of literals (zero or more)
                Production.DelimitedList(
                    Production.Lazy(() => Literal()),
                    TokenKind.Comma,
                    nodes => new NodeList(nodes)
                ).As("items"),
            
                // Match the closing bracket
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