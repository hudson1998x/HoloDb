using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Represents a grammar production in the Holo parsing engine.
    /// Productions define how sequences of syntax nodes are parsed.
    /// </summary>
    public abstract partial class Production
    {
        /// <summary>
        /// Creates a sequence production that runs multiple child productions in order.
        /// The result of the sequence is constructed using a builder function that
        /// receives any captured nodes by alias.
        /// </summary>
        /// <param name="children">The array of child <see cref="Production"/> instances to execute in sequence.</param>
        /// <param name="builder">
        /// A function that receives a dictionary of captured nodes (keyed by alias)
        /// and returns the resulting <see cref="SyntaxNode"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="Production"/> that represents the sequence of child productions.
        /// </returns>
        public static Production IsSequence(
            Production[] children,
            Func<Dictionary<string, SyntaxNode>, SyntaxNode> builder)
        {
            return new SequenceProduction(children, builder);
        }

        /// <summary>
        /// A private implementation of a sequence production.
        /// Executes a series of child productions in order and applies a builder function
        /// to the captured nodes to produce the final syntax node.
        /// </summary>
        private sealed class SequenceProduction : Production
        {
            /// <summary>
            /// The child productions to execute in sequence.
            /// </summary>
            private readonly Production[] _children;

            /// <summary>
            /// Builder function that constructs the final syntax node from captured nodes.
            /// </summary>
            private readonly Func<Dictionary<string, SyntaxNode>, SyntaxNode> _builder;

            /// <summary>
            /// Initializes a new instance of the <see cref="SequenceProduction"/> class.
            /// </summary>
            /// <param name="children">The child productions to execute in order.</param>
            /// <param name="builder">
            /// Function that takes captured nodes by alias and returns a <see cref="SyntaxNode"/>.
            /// </param>
            public SequenceProduction(Production[] children, Func<Dictionary<string, SyntaxNode>, SyntaxNode> builder)
            {
                _children = children;
                _builder = builder;
            }

            /// <summary>
            /// Attempts to parse the sequence of child productions from the current parser context position.
            /// If any child fails, rewinds the context to the start position and returns null.
            /// Captures the result of any aliased child productions and passes them to the builder.
            /// </summary>
            /// <param name="context">Reference to the <see cref="ParserContext"/> representing the current parse state.</param>
            /// <returns>
            /// The constructed <see cref="SyntaxNode"/> from the builder if all children succeed; otherwise, null.
            /// </returns>
            public override SyntaxNode? Parse(ref ParserContext context)
            {
                int startPos = context.Position;
                var captured = new Dictionary<string, SyntaxNode>();

                for (int i = 0; i < _children.Length; i++)
                {
                    var child = _children[i];
                    var node = child.Parse(ref context);
                    if (node == null)
                    {
                        context.Rewind(startPos);
                        return null;
                    }

                    // If child has an alias, store it
                    if (child is AliasedProduction aliased)
                        captured[aliased.Alias] = node;
                }

                return _builder(captured);
            }
        }
    }
}