namespace Holo.Sdk.Engine.Lexer
{
    /// <summary>
    /// Represents a single lexical token produced by <see cref="QueryLexer"/>.
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// The zero-based start position of the token in the input span.
        /// </summary>
        public int StartPosition;

        /// <summary>
        /// The zero-based end position of the token in the input span (exclusive).
        /// </summary>
        public int EndPosition;

        /// <summary>
        /// The type of token, as defined by <see cref="TokenKind"/>.
        /// </summary>
        public TokenKind Kind;

        /// <summary>
        /// Gets the length of the token (number of characters).
        /// </summary>
        public int Length => EndPosition - StartPosition;
    }
}