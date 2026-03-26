using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an invocation of a user-defined stored function.
/// This is different from <see cref="FunctionCallNode"/> which invokes built-in/runtime functions.
/// Example: <c>getUserRole($userId)</c>.
/// </summary>
public partial class FunctionInvocationNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the function being invoked.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode FunctionName { get; set; }

    /// <summary>
    /// Gets or sets the list of arguments passed to the function.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Arguments { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFunctionInvocationNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFunctionInvocationNode(this);
}