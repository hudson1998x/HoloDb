using System;
using System.Collections.Generic;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Partial abstract class for parser productions, including combinators for repetition patterns.
    /// </summary>
    public abstract partial class Production
    {
        /// <summary>
        /// Creates a production that parses zero or more occurrences of the given child production.
        /// </summary>
        /// <param name="child">The child production to repeat.</param>
        /// <param name="builder">
        /// Optional function to convert the list of parsed nodes into a custom <see cref="SyntaxNode"/>.
        /// If not provided, a <see cref="NodeList"/> is returned by default.
        /// </param>
        /// <returns>A <see cref="Production"/> that parses zero or more repetitions of the child.</returns>
        public static Production ZeroOrMore(Production child, Func<List<SyntaxNode>, SyntaxNode>? builder = null)
        {
            return new RepetitionProduction(child, 0, builder);
        }

        /// <summary>
        /// Creates a production that parses one or more occurrences of the given child production.
        /// </summary>
        /// <param name="child">The child production to repeat.</param>
        /// <param name="builder">
        /// Optional function to convert the list of parsed nodes into a custom <see cref="SyntaxNode"/>.
        /// If not provided, a <see cref="NodeList"/> is returned by default.
        /// </param>
        /// <returns>A <see cref="Production"/> that parses one or more repetitions of the child.</returns>
        public static Production OneOrMore(Production child, Func<List<SyntaxNode>, SyntaxNode>? builder = null)
        {
            return new RepetitionProduction(child, 1, builder);
        }

        /// <summary>
        /// A production that parses repeated occurrences of a child production with a minimum count.
        /// </summary>
        private sealed class RepetitionProduction : Production
        {
            private readonly Production _child;
            private readonly int _min;
            private readonly Func<List<SyntaxNode>, SyntaxNode>? _builder;

            /// <summary>
            /// Initializes a new instance of <see cref="RepetitionProduction"/>.
            /// </summary>
            /// <param name="child">The child production to repeat.</param>
            /// <param name="min">The minimum number of repetitions required.</param>
            /// <param name="builder">
            /// Optional function to convert parsed nodes into a custom <see cref="SyntaxNode"/>.
            /// </param>
            public RepetitionProduction(Production child, int min, Func<List<SyntaxNode>, SyntaxNode>? builder)
            {
                _child = child;
                _min = min;
                _builder = builder;
            }

            /// <summary>
            /// Parses repeated occurrences of the child production.
            /// Returns a <see cref="SyntaxNode"/> representing the repetitions, or <c>null</c> if
            /// fewer than the minimum required repetitions were matched.
            /// </summary>
            /// <param name="context">The parser context.</param>
            /// <returns>
            /// A <see cref="SyntaxNode"/> containing the parsed nodes (via <see cref="_builder"/> or <see cref="NodeList"/>)
            /// or <c>null</c> if the minimum repetitions were not met.
            /// </returns>
            public override SyntaxNode? Parse(ref ParserContext context)
            {
                int startPos = context.Position;
                var nodes = new List<SyntaxNode>();

                while (true)
                {
                    int posBefore = context.Position;
                    var node = _child.Parse(ref context);
                    if (node == null) break; // stop on first failure
                    nodes.Add(node);

                    // avoid infinite loop if production consumes nothing
                    if (context.Position == posBefore) break;
                }

                if (nodes.Count < _min)
                {
                    context.Rewind(startPos);
                    return null;
                }

                return _builder != null ? _builder(nodes) : new NodeList(nodes);
            }
        }
    }
}