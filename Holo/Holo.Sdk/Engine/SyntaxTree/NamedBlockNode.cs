namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a named block in the syntax tree, which can contain a collection of fields.
/// For example: <c>person { name, age, address }</c>.
/// </summary>
public partial class NamedBlockNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the block.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Name { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of fields contained in the block.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Fields { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitNamedBlockNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitNamedBlockNode(this);
}