using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a syntax node that assigns an alias to another <see cref="SyntaxNode"/>.
/// </summary>
/// <remarks>
/// An <see cref="AliasedNode"/> is typically used in constructs where a value,
/// expression, or symbol is given an alternate name within the syntax tree.
/// </remarks>
public partial class AliasedNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the underlying syntax node being aliased.
    /// </summary>
    public required SyntaxNode Left;
    
    /// <summary>
    /// Gets or sets the token representing the alias identifier.
    /// </summary>
    public required IdentifierNode Alias { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitAliasedNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitAliasedNode(this);
}