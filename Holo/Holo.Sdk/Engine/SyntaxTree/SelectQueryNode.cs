namespace Holo.Sdk.Engine.SyntaxTree
{
    /// <summary>
    /// Represents a "SELECT" query node in the syntax tree.
    /// </summary>
    /// <remarks>
    /// This node is used to model a query that retrieves specific fields
    /// from a target collection within the Holo SDK engine. It contains
    /// the collection being queried and the list of fields to select.
    /// </remarks>
    public class SelectQueryNode : SyntaxNode
    {
        /// <summary>
        /// Gets or sets the name of the target collection to query.
        /// </summary>
        /// <value>
        /// A required string representing the collection name.
        /// </value>
        public required string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the list of fields to select from the target collection.
        /// </summary>
        /// <value>
        /// A required list of <see cref="SyntaxNode"/> instances representing
        /// each field to be retrieved in the query.
        /// </value>
        public required List<SyntaxNode> SelectFields { get; set; }
    }
}