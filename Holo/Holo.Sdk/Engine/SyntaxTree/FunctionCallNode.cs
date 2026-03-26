namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a function call in the syntax tree, including the function name
/// and its argument list. For example: <c>sum(values)</c>.
/// </summary>
public partial class FunctionCallNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the function being called.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode FunctionName { get; set; }
    
    /// <summary>
    /// Gets or sets the list of arguments passed to the function.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Arguments { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFunctionCallNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFunctionCallNode(this);
}