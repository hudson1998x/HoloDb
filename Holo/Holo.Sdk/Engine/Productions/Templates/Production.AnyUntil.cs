using System;
using System.Collections.Generic;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions
{
    public abstract partial class Production
    {
        /// <summary>
        /// Matches any tokens until the given terminator token kind is reached.
        /// The user-provided lambda decides what concrete SyntaxNode to produce.
        /// </summary>
        /// <param name="terminator">Token kind to stop at.</param>
        /// <param name="builder">
        /// Lambda that takes the captured tokens and returns a concrete SyntaxNode.
        /// </param>
        public static Production AnyUntil(TokenKind terminator, Func<List<Token>, SyntaxNode> builder)
        {
            return new AnyUntilProduction(terminator, builder);
        }

        private sealed class AnyUntilProduction : Production
        {
            private readonly TokenKind _terminator;
            private readonly Func<List<Token>, SyntaxNode> _builder;

            public AnyUntilProduction(TokenKind terminator, Func<List<Token>, SyntaxNode> builder)
            {
                _terminator = terminator;
                _builder = builder;
            }

            public override SyntaxNode? Parse(ref ParserContext context)
            {
                int startPos = context.Position;
                var tokens = new List<Token>();

                while (!context.IsAtEnd)
                {
                    var token = context.Peek();
                    if (!token.HasValue || token.Value.Kind == _terminator)
                        break;

                    // consume the token
                    context.Position++;
                    tokens.Add(token.Value); // <-- careful: just add the token itself
                }

                if (tokens.Count == 0)
                {
                    context.Rewind(startPos);
                    return null;
                }

                // user lambda decides the concrete node type
                return _builder(tokens);
            }
        }
    }
}