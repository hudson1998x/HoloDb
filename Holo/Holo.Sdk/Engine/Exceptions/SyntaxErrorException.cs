using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a syntax error is encountered during parsing.
/// </summary>
public class SyntaxErrorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxErrorException"/> class
    /// using the unexpected token and the original source input.
    /// </summary>
    /// <param name="unexpectedToken">
    /// The token that caused the syntax error.
    /// </param>
    /// <param name="source">
    /// The full source text being parsed.
    /// </param>
    public SyntaxErrorException(Token unexpectedToken, in ReadOnlySpan<char> source) :
        base(BuildErrorMessage(unexpectedToken, source))
    {
    }
    
    /// <summary>
    /// Builds a detailed error message including the token text, token kind,
    /// and its location (line and column) in the source.
    /// </summary>
    /// <param name="token">The token that caused the error.</param>
    /// <param name="source">The full source text.</param>
    /// <returns>A formatted error message string.</returns>
    private static string BuildErrorMessage(Token token, in ReadOnlySpan<char> source)
    {
        var (line, column) = GetLineAndColumn(source, token.StartPosition);
        var tokenText = source.Slice(token.StartPosition, token.Length).ToString();
        
        return $"Unexpected token: '{tokenText}' ({token.Kind}) at line {line}, column {column}";
    }
    
    /// <summary>
    /// Calculates the line and column number corresponding to a character position
    /// within the source text.
    /// </summary>
    /// <param name="source">The full source text.</param>
    /// <param name="position">The zero-based character index.</param>
    /// <returns>
    /// A tuple containing the 1-based line and column numbers.
    /// </returns>
    private static (int line, int column) GetLineAndColumn(in ReadOnlySpan<char> source, int position)
    {
        int line = 1;
        int column = 1;
        
        for (int i = 0; i < position && i < source.Length; i++)
        {
            if (source[i] == '\n')
            {
                line++;
                column = 1;
            }
            else
            {
                column++;
            }
        }
        
        return (line, column);
    }
}