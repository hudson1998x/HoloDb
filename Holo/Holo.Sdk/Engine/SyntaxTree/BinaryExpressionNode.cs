using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a binary expression in the syntax tree, consisting of a left-hand side,
/// an operator, and a right-hand side. For example: <c>a + b</c>.
/// </summary>
public partial class BinaryExpressionNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the left-hand side of the binary expression.
    /// This property is required and must be initialized.
    /// </summary>
    public required SyntaxNode Left;

    /// <summary>
    /// Gets or sets the operator of the binary expression.
    /// This is typically an <see cref="IdentifierNode"/> representing symbols like '+', '-', '*', '/'.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Operator;
    
    /// <summary>
    /// Gets or sets the right-hand side of the binary expression.
    /// This property is required and must be initialized.
    /// </summary>
    public required SyntaxNode Right;
}