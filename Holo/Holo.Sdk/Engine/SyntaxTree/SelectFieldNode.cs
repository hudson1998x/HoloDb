namespace Holo.Sdk.Engine.SyntaxTree
{
    /// <summary>
    /// Represents a single field in a SELECT query.
    /// </summary>
    /// <remarks>
    /// This node is used within a <see cref="SelectQueryNode"/> to define
    /// which fields are being selected. An optional alias can be provided
    /// for renaming the field in the query results.
    /// </remarks>
    public class SelectFieldNode : SyntaxNode
    {
        /// <summary>
        /// Gets or sets the optional alias for the selected field.
        /// </summary>
        /// <value>
        /// A string representing the alias. Can be <c>null</c> if no alias is used.
        /// </value>
        public string? Alias { get; set; }
        
        /// <summary>
        /// Gets or sets the syntax node representing the actual field being selected.
        /// </summary>
        /// <value>
        /// A required <see cref="SyntaxNode"/> representing the field.
        /// </value>
        public required SyntaxNode Field { get; set; }
    }
}