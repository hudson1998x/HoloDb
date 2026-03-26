namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a property of a field definition.
/// Examples: <c>type(int)</c>, <c>default(auto_increment)</c>, <c>primary()</c>, <c>validate(notEmpty)</c>
/// </summary>
public sealed partial class FieldPropertyNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the property (e.g., "type", "default", "primary", "null", "validate").
    /// </summary>
    public required IdentifierNode PropertyName { get; init; }

    /// <summary>
    /// Gets or sets the arguments passed to this property (e.g., "int" in type(int), "notEmpty" in validate(notEmpty)).
    /// Empty for properties like primary() and unique().
    /// </summary>
    public required NodeList Arguments { get; init; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFieldPropertyNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFieldPropertyNode(this);
}
