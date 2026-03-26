namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a filter expression, which can take one of several forms:
    /// <list type="bullet">
    /// <item><description><see cref="FilterBinary"/> – binary expressions like <c>expr AND expr</c> or <c>expr OR expr</c>.</description></item>
    /// <item><description><see cref="FilterGroup"/> – parenthesized expressions <c>( ... )</c>.</description></item>
    /// <item><description><see cref="FilterList"/> – comma-separated rules like <c>above 20, below 40</c>.</description></item>
    /// <item><description><see cref="FilterRule"/> – a single filter rule like <c>above 20</c>.</description></item>
    /// </list>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that parses a filter expression and returns the appropriate <see cref="SyntaxNode"/> subtype.
    /// </returns>
    public static Production FilterExpression()
    {
        return Production.Choice(
            FilterBinary(), // expr OR expr, expr AND expr
            FilterGroup(),  // ( ... )
            FilterList(),   // above 20, below 40  <-- comma-separated rules
            FilterRule()    // above 20
        );
    }
}