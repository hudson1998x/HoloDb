namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a "WHERE" clause in a syntax tree, typically used for filtering data or expressions.
/// Contains a list of filter nodes that define the conditions of the clause.
/// </summary>
public partial class WhereClauseNode : SyntaxNode
{
    /// <summary>
    /// The list of filter nodes contained in this WHERE clause.
    /// Each node in <see cref="Filters"/> represents a condition or a logical grouping of conditions.
    /// </summary>
    public required NodeList Filters;

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitWhereClauseNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitWhereClauseNode(this);
}