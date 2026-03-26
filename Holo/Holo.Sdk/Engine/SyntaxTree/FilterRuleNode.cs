namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a single filter rule in the syntax tree, consisting of an operator
/// and a value. For example: <c>age &gt; 18</c>.
/// </summary>
public partial class FilterRuleNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the operator of the filter rule (e.g., &gt;, &lt;, ==).
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Operator { get; set; }
    
    /// <summary>
    /// Gets or sets the value being compared or filtered against.
    /// This property is required and must be initialized.
    /// </summary>
    public required SyntaxNode Value { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFilterRuleNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFilterRuleNode(this);
}