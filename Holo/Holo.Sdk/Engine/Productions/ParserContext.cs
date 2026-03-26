using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.Productions
{
    /// <summary>
    /// Represents the parsing context for a token stream.
    /// This is a stack-only mutable parser context ref struct. 
    /// used for building parsers that can backtrack and track syntax errors.
    /// </summary>
    public ref struct ParserContext
    {
        private readonly ReadOnlySpan<Token> _tokens;

        /// <summary>
        /// Current position in the token stream.
        /// This is mutable to allow advancing and rewinding during parsing.
        /// </summary>
        public int Position;

        /// <summary>
        /// Furthest position reached during a failed match.
        /// Useful for reporting the location of syntax errors.
        /// </summary>
        public int MaxFailedPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserContext"/> struct
        /// with the given token stream.
        /// </summary>
        /// <param name="tokens">The token stream to parse.</param>
        public ParserContext(ReadOnlySpan<Token> tokens)
        {
            _tokens = tokens;
            Position = 0;
            MaxFailedPosition = 0;
        }

        /// <summary>
        /// Attempts to match a token of the specified <paramref name="kind"/> at the current position.
        /// </summary>
        /// <param name="kind">The kind of token to match.</param>
        /// <param name="token">The matched token, if successful; otherwise, default.</param>
        /// <returns>True if the token matches; otherwise, false.</returns>
        /// <remarks>
        /// Advances the <see cref="Position"/> by one if successful.
        /// Updates <see cref="MaxFailedPosition"/> on failure.
        /// </remarks>
        public bool TryMatch(TokenKind kind, out Token token)
        {
            if (Position < _tokens.Length && _tokens[Position].Kind == kind)
            {
                token = _tokens[Position];
                Position++;
                return true;
            }

            token = default;
            MaxFailedPosition = Math.Max(MaxFailedPosition, Position);
            return false;
        }

        /// <summary>
        /// Rewinds the current position to a previous location in the token stream.
        /// </summary>
        /// <param name="position">The position to rewind to.</param>
        public void Rewind(int position) => Position = position;

        /// <summary>
        /// Gets a value indicating whether the parser has reached the end of the token stream.
        /// </summary>
        public bool IsAtEnd => Position >= _tokens.Length;

        /// <summary>
        /// Peeks at the current token without advancing the <see cref="Position"/>.
        /// </summary>
        /// <returns>The current token, or null if at the end of the token stream.</returns>
        public Token? Peek() => Position < _tokens.Length ? _tokens[Position] : null;
    }
}