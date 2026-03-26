using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a binary filter expression of the form <c>left op right</c>,
    /// where <c>op</c> is typically a logical operator like "and" or "or".
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="BinaryExpressionNode"/>
    /// representing the parsed binary filter expression.
    /// </returns>
    public static Production FilterBinary()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Left-hand side of the binary expression
                Production.Lazy(() => FilterTerm()).As("left"),

                // Operator token, e.g., "and" or "or"
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("op"),

                // Right-hand side of the binary expression
                Production.Lazy(() => FilterExpression()).As("right")
            },
            captured =>
            {
                return new BinaryExpressionNode
                {
                    Left = captured["left"],
                    Operator = (IdentifierNode)captured["op"],
                    Right = captured["right"]
                };
            }
        );
    }
}