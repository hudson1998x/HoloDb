namespace Holo.Sdk.Engine.SyntaxTree
{
    /// <summary>
    /// Represents an identifier in the syntax tree.
    /// </summary>
    /// <remarks>
    /// An <see cref="IdentifierNode"/> is typically used to represent names
    /// of collections, fields, or aliases within queries in the Holo SDK engine.
    /// </remarks>
    public class IdentifierNode : SyntaxNode
    {
        /// <summary>
        /// Gets or sets the string value of the identifier.
        /// </summary>
        /// <value>
        /// A required <see cref="string"/> representing the name or alias.
        /// </value>
        public required string Value { get; set; }
    }
}