using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a parenthesized filter expression, e.g., <c>( ... )</c>.
    /// The parentheses are used for grouping but are not preserved in the returned node.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that parses a parenthesized filter expression and returns
    /// the underlying <see cref="SyntaxNode"/> inside the group.
    /// </returns>
    public static Production FilterGroup()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Match opening parenthesis
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                // Parse the inner filter expression
                Production.Lazy(() => FilterExpression()).As("expr"),

                // Match closing parenthesis
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            // Unwrap the inner expression and return it directly
            captured => captured["expr"]
        );
    }
}