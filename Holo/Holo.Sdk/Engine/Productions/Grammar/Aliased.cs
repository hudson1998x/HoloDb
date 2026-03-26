using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Creates a production for parsing an aliased field.
    /// Matches a base field followed by an "as" keyword and an alias identifier.
    /// Example: <c>fieldName as aliasName</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that parses an aliased field and returns an <see cref="AliasedNode"/>.
    /// </returns>
    public static Production AliasedField()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Use BaseField, not Field!
                Production.Lazy(() => BaseField()).As("field"),  
            
                // Expect the "as" keyword followed by an alias identifier
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode() { Value = t }).As("as"),
            
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode() { Value = t }).As("alias")
            },
            captured =>
            {
                return new AliasedNode
                {
                    Left = captured["field"],
                    Alias = (IdentifierNode)captured["alias"]
                };
            }
        );
    }
}