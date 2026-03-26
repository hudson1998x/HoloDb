using System;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Partial abstract class for parser productions, including combinators for optional patterns.
    /// </summary>
    public abstract partial class Production
    {
        /// <summary>
        /// Wraps a production to make it optional.
        /// Matches zero or one occurrence of the given child production.
        /// </summary>
        /// <param name="child">The production to make optional.</param>
        /// <returns>A <see cref="Production"/> that may match the child or nothing.</returns>
        public static Production Optional(Production child)
        {
            return new OptionalProduction(child);
        }

        /// <summary>
        /// A production that attempts to parse a child production optionally.
        /// </summary>
        private sealed class OptionalProduction : Production
        {
            private readonly Production _child;

            /// <summary>
            /// Initializes a new instance of <see cref="OptionalProduction"/>.
            /// </summary>
            /// <param name="child">The production to make optional.</param>
            public OptionalProduction(Production child)
            {
                _child = child;
            }

            /// <summary>
            /// Attempts to parse the child production.
            /// Returns the node if matched, or <c>null</c> if not matched, without failing.
            /// </summary>
            /// <param name="context">The parser context.</param>
            /// <returns>
            /// The parsed <see cref="SyntaxNode"/> if the child matches; otherwise, <c>null</c>.
            /// </returns>
            public override SyntaxNode? Parse(ref ParserContext context)
            {
                int startPos = context.Position;

                var node = _child.Parse(ref context);
                if (node != null)
                {
                    return node; // child matched, return the node
                }

                // child did not match — rewind and succeed with null
                context.Rewind(startPos);
                return null;
            }
        }
    }
}