using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a literal value in the syntax tree, such as a number, string, or boolean.
/// </summary>
public partial class LiteralNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the token representing the literal value.
    /// This property is required and must be initialized.
    /// </summary>
    public required Token Value { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitLiteralNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitLiteralNode(this);
}