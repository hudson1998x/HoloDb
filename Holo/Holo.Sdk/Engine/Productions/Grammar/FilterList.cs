using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a comma-separated list of filter rules or groups.
    /// Multiple rules separated by commas are implicitly treated as logical AND.
    /// Example: <c>above 20, below 40</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FilterListNode"/>
    /// containing the parsed rules as a <see cref="NodeList"/>.
    /// </returns>
    public static Production FilterList()
    {
        return Production.DelimitedList(
            Production.Choice(
                FilterGroup(),
                FilterRule()
            ),
            TokenKind.Comma,
            nodes =>
            {
                // Multiple rules separated by commas are implicitly AND-ed
                return new FilterListNode
                {
                    Rules = new NodeList(nodes)
                };
            }
        );
    }
}