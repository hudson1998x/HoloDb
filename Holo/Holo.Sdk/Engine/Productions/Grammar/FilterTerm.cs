namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterTerm()
    {
        return Production.Choice(
            FilterGroup(),
            FilterRule()
        );
    }
}