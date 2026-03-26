using System;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Partial abstract class for parser productions, including token-level parsing helpers.
    /// </summary>
    public abstract partial class Production
    {
        /// <summary>
        /// Creates a production that matches a single token of the specified kind.
        /// When the token is matched, the provided lambda is invoked to produce
        /// the corresponding <see cref="SyntaxNode"/>.
        /// </summary>
        /// <param name="kind">The <see cref="TokenKind"/> to match.</param>
        /// <param name="onMatch">A lambda that produces a <see cref="SyntaxNode"/> from the matched token.</param>
        /// <returns>A <see cref="Production"/> that matches a single token and returns a node.</returns>
        public static Production TokenIs(TokenKind kind, Func<Token, SyntaxNode> onMatch)
        {
            return new TokenProduction(kind, onMatch);
        }

        /// <summary>
        /// A production that matches a single token of a specific kind and produces a <see cref="SyntaxNode"/>.
        /// </summary>
        private sealed class TokenProduction : Production
        {
            private readonly TokenKind _kind;
            private readonly Func<Token, SyntaxNode> _onMatch;

            /// <summary>
            /// Initializes a new instance of <see cref="TokenProduction"/>.
            /// </summary>
            /// <param name="kind">The token kind to match.</param>
            /// <param name="onMatch">A function to produce a <see cref="SyntaxNode"/> from the matched token.</param>
            public TokenProduction(TokenKind kind, Func<Token, SyntaxNode> onMatch)
            {
                _kind = kind;
                _onMatch = onMatch;
            }

            /// <summary>
            /// Attempts to parse a single token of the specified kind.
            /// If successful, returns the <see cref="SyntaxNode"/> produced by <see cref="_onMatch"/>.
            /// Otherwise, returns <c>null</c> and rewinds the parser position.
            /// </summary>
            /// <param name="context">The parser context.</param>
            /// <returns>
            /// The <see cref="SyntaxNode"/> representing the matched token, or <c>null</c> if no match.
            /// </returns>
            public override SyntaxNode? Parse(ref ParserContext context)
            {
                int startPos = context.Position;

                if (!context.TryMatch(_kind, out var token))
                {
                    context.Rewind(startPos);
                    return null;
                }

                // the lambda produces the concrete node
                var node = _onMatch(token);

                return node; // alias wrapping is handled by AliasedProduction
            }
        }
    }
}