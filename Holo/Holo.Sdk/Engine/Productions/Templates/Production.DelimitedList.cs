using System;
using System.Collections.Generic;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial class for parser productions, including helpers for common parsing patterns.
/// </summary>
public partial class Production
{
    /// <summary>
    /// Creates a production that parses a list of items separated by a specific token.
    /// For example, parsing <c>a, b, c</c> with <c>','</c> as the separator.
    /// </summary>
    /// <param name="item">The production representing a single list item.</param>
    /// <param name="separator">The token kind that separates items in the list.</param>
    /// <param name="builder">
    /// Optional function to convert the list of parsed nodes into a custom <see cref="SyntaxNode"/>.
    /// If not provided, a <see cref="NodeList"/> is returned by default.
    /// </param>
    /// <returns>A <see cref="Production"/> that parses a delimited list of items.</returns>
    public static Production DelimitedList(Production item, TokenKind separator, Func<List<SyntaxNode>, SyntaxNode>? builder = null)
    {
        return new DelimitedListProduction(item, separator, builder);
    }

    /// <summary>
    /// A production that parses a list of items separated by a specific token.
    /// </summary>
    private sealed class DelimitedListProduction : Production
    {
        private readonly Production _item;
        private readonly TokenKind _separator;
        private readonly Func<List<SyntaxNode>, SyntaxNode>? _builder;

        /// <summary>
        /// Initializes a new instance of <see cref="DelimitedListProduction"/>.
        /// </summary>
        /// <param name="item">The production representing a single list item.</param>
        /// <param name="separator">The token kind separating list items.</param>
        /// <param name="builder">
        /// Optional function to convert parsed items into a custom <see cref="SyntaxNode"/>.
        /// </param>
        public DelimitedListProduction(Production item, TokenKind separator, Func<List<SyntaxNode>, SyntaxNode>? builder)
        {
            _item = item;
            _separator = separator;
            _builder = builder;
        }

        /// <summary>
        /// Attempts to parse a delimited list of items from the input tokens.
        /// Returns a <see cref="SyntaxNode"/> representing the list or <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="context">The parser context.</param>
        /// <returns>
        /// A <see cref="SyntaxNode"/> representing the parsed list (custom via <see cref="_builder"/> or <see cref="NodeList"/>)
        /// or <c>null</c> if no items could be parsed.
        /// </returns>
        public override SyntaxNode? Parse(ref ParserContext context)
        {
            int startPos = context.Position;
            var nodes = new List<SyntaxNode>();

            // first item
            var first = _item.Parse(ref context);
            if (first == null)
            {
                context.Rewind(startPos);
                return null; // fail if no items
            }
            nodes.Add(first);

            while (!context.IsAtEnd)
            {
                int posBefore = context.Position;

                // check for separator token
                var nextToken = context.Peek();
                if (!nextToken.HasValue || nextToken.Value.Kind != _separator)
                    break;

                // consume separator
                context.Position++;

                // parse next item
                var next = _item.Parse(ref context);
                if (next == null)
                {
                    // rollback to before separator if next item fails
                    context.Rewind(posBefore);
                    break;
                }

                nodes.Add(next);

                // safety: avoid infinite loop if production consumes nothing
                if (context.Position == posBefore) break;
            }

            if (nodes.Count == 0)
            {
                context.Rewind(startPos);
                return null;
            }

            return _builder != null ? _builder(nodes) : new NodeList(nodes);
        }
    }
}