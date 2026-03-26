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

        /// <summary>
        /// Gets the text representation of the token.
        /// </summary>
        public string? Text { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> struct.
        /// </summary>
        /// <param name="text">The text representation of the token.</param>
        public Token(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Token"/>.
    /// </summary>
    public static class TokenExtensions
    {
        /// <summary>
        /// Gets the text representation of the token from the source span.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="source">The source text.</param>
        /// <returns>The token text.</returns>
        public static string GetText(this Token token, in ReadOnlySpan<char> source)
        {
            return source.Slice(token.StartPosition, token.Length).ToString();
        }
    }
}