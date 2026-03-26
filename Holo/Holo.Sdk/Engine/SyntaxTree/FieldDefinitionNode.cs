namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a field definition within a type definition.
/// Example: <c>id { type(int), default(auto_increment), primary(), ... }</c>
/// </summary>
public sealed partial class FieldDefinitionNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    public required IdentifierNode FieldName { get; init; }

    /// <summary>
    /// Gets or sets the properties of this field (type, default, primary, null, etc.).
    /// </summary>
    public required NodeList Properties { get; init; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFieldDefinitionNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFieldDefinitionNode(this);
}
