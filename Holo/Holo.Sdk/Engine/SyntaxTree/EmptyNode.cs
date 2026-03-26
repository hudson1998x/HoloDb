namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an empty or placeholder node in the syntax tree.
/// Can be used in situations where a node is syntactically required
/// but no actual content is present.
/// </summary>
public class EmptyNode : SyntaxNode
{
    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitEmptyNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitEmptyNode(this);
}