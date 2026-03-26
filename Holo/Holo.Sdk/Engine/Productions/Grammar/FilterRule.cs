using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a single filter rule of the form <c>operator value</c>.
    /// The operator can be either a keyword (e.g., <c>in</c>) or a generic identifier.
    /// The value can be a literal (number, string, etc.) or an array literal.
    /// Examples:
    /// <list type="bullet">
    /// <item><c>above 20</c></item>
    /// <item><c>status ['completed', 'dispatched']</c></item>
    /// </list>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FilterRuleNode"/>
    /// containing the parsed operator and value.
    /// </returns>
    public static Production FilterRule()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Operator: either a keyword or an identifier
                Production.Choice(
                    Production.TokenIs(TokenKind.KeywordIn, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t })
                ).As("operator"),
            
                // Value: either an array literal or a single literal
                Production.Choice(
                    ArrayLiteral(),                 // e.g., ['completed', 'dispatched']
                    Production.Lazy(() => Literal()) // e.g., 20, 'string', etc.
                ).As("value")
            },
            captured =>
            {
                return new FilterRuleNode
                {
                    Operator = (IdentifierNode)captured["operator"],
                    Value = captured["value"]
                };
            }
        );
    }
}