namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an attribute applied to a type definition.
/// Examples: <c>@name('User')</c>, <c>@description('This is the user entity')</c>, <c>@timestamps()</c>
/// </summary>
public sealed partial class AttributeNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the attribute (without the @ symbol).
    /// </summary>
    public required IdentifierNode AttributeName { get; init; }

    /// <summary>
    /// Gets or sets the arguments passed to this attribute.
    /// Empty for attributes like @timestamps().
    /// </summary>
    public required NodeList Arguments { get; init; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitAttributeNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitAttributeNode(this);
}
