using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Contains grammar productions for parsing Holo queries and expressions.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Defines the production for a "WHERE" clause in a query.
    /// Matches the keyword <c>WHERE</c>, an opening bracket,
    /// a list of filter fields, and a closing bracket.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that parses a <see cref="WhereClauseNode"/>,
    /// including its <see cref="WhereClauseNode.Filters"/>.
    /// </returns>
    public static Production WhereClause()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Match the 'WHERE' keyword and create an IdentifierNode for it
                Production.TokenIs(TokenKind.KeywordWhere, t => new IdentifierNode { Value = t }),

                // Match opening bracket '[' (empty node for structure)
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),

                // Parse fields inside the WHERE block lazily and assign as "filters"
                Production.Lazy(() => Fields()).As("filters"),

                // Match closing bracket ']' (empty node for structure)
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                // Construct the WhereClauseNode and set Filters from captured fields
                return new WhereClauseNode
                {
                    Filters = (NodeList)captured["filters"]
                };
            }
        );
    }
}