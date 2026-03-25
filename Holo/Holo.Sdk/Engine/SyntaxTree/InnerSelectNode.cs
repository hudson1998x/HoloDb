namespace Holo.Sdk.Engine.SyntaxTree
{
    /// <summary>
    /// Represents an inner select expression within a query.
    /// </summary>
    /// <remarks>
    /// This node is typically used for subqueries or computed expressions
    /// inside a <see cref="SelectQueryNode"/>. It can optionally have an
    /// identifier (alias) and must have an expression that defines the
    /// inner selection logic.
    /// </remarks>
    public class InnerSelectNode : SyntaxNode
    {
        /// <summary>
        /// Gets or sets the optional identifier (alias) for this inner select.
        /// </summary>
        /// <value>
        /// An <see cref="IdentifierNode"/> representing the alias, or <c>null</c>
        /// if no alias is provided.
        /// </value>
        public required IdentifierNode? Identifier { get; set; }
        
        /// <summary>
        /// Gets or sets the expression that defines this inner selection.
        /// </summary>
        /// <value>
        /// A required <see cref="SyntaxNode"/> representing the inner expression.
        /// </value>
        public required SyntaxNode Expression { get; set; }
    }
}