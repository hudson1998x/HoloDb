namespace Holo.Sdk.Engine.SyntaxTree
{
    /// <summary>
    /// Represents the base class for all nodes in a syntax tree.
    /// </summary>
    /// <remarks>
    /// This class is abstract and intended to be inherited by specific types of
    /// syntax nodes within the Holo SDK engine. It defines the common interface
    /// and behavior shared across all syntax nodes using the visitor pattern.
    /// </remarks>
    public abstract partial class SyntaxNode
    {
        /// <summary>
        /// Accepts a generic visitor for traversal with a return value.
        /// </summary>
        public abstract TResult Accept<TResult>(IVisitor<TResult> visitor);

        /// <summary>
        /// Accepts a void visitor for traversal.
        /// </summary>
        public abstract void Accept(IVisitor visitor);
    }
}