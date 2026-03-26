namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a single filter term, which can be either:
    /// <list type="bullet">
    /// <item><description>A parenthesized filter group (see <see cref="FilterGroup"/>).</description></item>
    /// <item><description>A single filter rule (see <see cref="FilterRule"/>).</description></item>
    /// </list>
    /// This is the basic unit used in filter expressions and binary filter operations.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="SyntaxNode"/> representing
    /// either a filter group or a single filter rule.
    /// </returns>
    public static Production FilterTerm()
    {
        return Production.Choice(
            FilterGroup(),
            FilterRule()
        );
    }
}