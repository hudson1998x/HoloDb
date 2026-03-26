namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterExpression()
    {
        return Production.Choice(
            FilterBinary(), // expr OR expr, expr AND expr
            FilterGroup(), // ( ... )
            FilterList(), // above 20, below 40  <-- NEW: comma-separated rules
            FilterRule() // above 20
        );
    }
}