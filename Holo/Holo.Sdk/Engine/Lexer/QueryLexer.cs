namespace Holo.Sdk.Engine.Lexer
{
    /// <summary>
    /// Provides lexical analysis functionality for parsing query strings into tokens.
    /// </summary>
    public static partial class QueryLexer
    {
        /// <summary>
        /// Parses the input span into an array of <see cref="Token"/> instances.
        /// </summary>
        /// <param name="input">The input character span to tokenize.</param>
        /// <returns>An array of <see cref="Token"/> representing the lexed input.</returns>
        public static Token[] Parse(ReadOnlySpan<char> input)
        {
            // Pre-allocate max tokens = input length
            var tokens = new Token[input.Length];
            int tokenCount = 0;
            int pointer = 0;

            while (pointer < input.Length)
            {
                char c = input[pointer];

                if (char.IsWhiteSpace(c))
                {
                    pointer++;
                    continue;
                }

                if (TrySymbol(input, ref pointer, tokens, ref tokenCount))
                    continue;

                if (TryKeyword(input, ref pointer, tokens, ref tokenCount))
                    continue;

                if (IsDigit(c))
                {
                    ParseNumber(input, ref pointer, tokens, ref tokenCount);
                    continue;
                }

                if (IsIdentifierStart(c))
                {
                    ParseIdentifier(input, ref pointer, tokens, ref tokenCount);
                    continue;
                }

                pointer++; // skip unknown char
            }

            return tokens[..tokenCount];
        }

        /// <summary>
        /// Attempts to parse a single-character or quoted string symbol at the current pointer.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        /// <returns>True if a symbol was successfully parsed; otherwise, false.</returns>
        private static bool TrySymbol(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            char c = span[pointer];
            TokenKind kind;
            switch (c)
            {
                case '[': kind = TokenKind.LeftSquareBracket; break;
                case ']': kind = TokenKind.RightSquareBracket; break;
                case '(': kind = TokenKind.LeftParenthesis; break;
                case ')': kind = TokenKind.RightParenthesis; break;
                case ',': kind = TokenKind.Comma; break;
                case '{': kind = TokenKind.LeftBracket; break;
                case '}': kind = TokenKind.RightBracket; break;
                case '<': kind = TokenKind.LessThan; break;
                case '>': kind = TokenKind.MoreThan; break;
                case '!': kind = TokenKind.KeywordNot; break;
                case '=': kind = TokenKind.Equal; break;
                case '$': kind = TokenKind.DollarSign; break;
                case ':': kind = TokenKind.Colon; break;
                case '\'': ParseSingleQuotedString(span, ref pointer, tokens, ref tokenCount); return true;
                case '"': ParseDoubleQuotedString(span, ref pointer, tokens, ref tokenCount); return true;
                default: return false;
            }

            tokens[tokenCount++] = new Token { Kind = kind, StartPosition = pointer, EndPosition = pointer + 1 };
            pointer++;
            return true;
        }

        /// <summary>
        /// Attempts to match a keyword at the current pointer.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        /// <returns>True if a keyword was successfully matched; otherwise, false.</returns>
        private static bool TryKeyword(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            ReadOnlySpan<char> input = span.Slice(pointer);
            string[] keywords =
            {
                "endsWith", "equals", "is", "contains", "null", "not",
                "false", "startsWith", "true", "within", "moreThan",
                "lessThan", "in", "where", "function", "return"
            };
            TokenKind[] kinds =
            {
                TokenKind.KeywordEndsWith, TokenKind.Equal, TokenKind.KeywordIs,
                TokenKind.KeywordContains, TokenKind.NullLiteral, TokenKind.KeywordNot,
                TokenKind.BooleanLiteral, TokenKind.KeywordStartsWith, TokenKind.BooleanLiteral,
                TokenKind.KeywordWithin, TokenKind.MoreThan, TokenKind.LessThan, TokenKind.KeywordIn,
                TokenKind.KeywordWhere, TokenKind.KeywordFunction, TokenKind.KeywordReturn
            };

            for (int i = 0; i < keywords.Length; i++)
            {
                var kw = keywords[i];
                if (input.StartsWith(kw, StringComparison.Ordinal))
                {
                    // boundary check so "is" doesn't match "island"
                    if (kw.Length < input.Length && IsIdentifierPart(input[kw.Length]))
                        continue;

                    tokens[tokenCount++] = new Token
                    {
                        Kind = kinds[i],
                        StartPosition = pointer,
                        EndPosition = pointer + kw.Length
                    };
                    pointer += kw.Length;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Parses a numeric literal (integer or decimal) from the input.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        private static void ParseNumber(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            int start = pointer;
            while (pointer < span.Length && IsDigit(span[pointer])) pointer++;
            if (pointer < span.Length && span[pointer] == '.')
            {
                pointer++;
                while (pointer < span.Length && IsDigit(span[pointer])) pointer++;
            }
            tokens[tokenCount++] = new Token { Kind = TokenKind.NumberLiteral, StartPosition = start, EndPosition = pointer };
        }

        /// <summary>
        /// Parses an identifier (variable or property name) from the input.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        private static void ParseIdentifier(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            int start = pointer;
            while (pointer < span.Length && IsIdentifierPart(span[pointer])) pointer++;
            tokens[tokenCount++] = new Token { Kind = TokenKind.Identifier, StartPosition = start, EndPosition = pointer };
        }

        /// <summary>
        /// Parses a single-quoted string literal.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        private static void ParseSingleQuotedString(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            int start = pointer;
            pointer++;
            while (pointer < span.Length && span[pointer] != '\'') pointer++;

            bool closed = pointer < span.Length && span[pointer] == '\'';
            if (closed) pointer++;

            tokens[tokenCount++] = new Token
            {
                Kind = closed ? TokenKind.StringLiteral : TokenKind.Error,
                StartPosition = start,
                EndPosition = pointer
            };
        }

        /// <summary>
        /// Parses a double-quoted string literal.
        /// </summary>
        /// <param name="span">The input span.</param>
        /// <param name="pointer">Reference to the current pointer in the span.</param>
        /// <param name="tokens">The array to store parsed tokens.</param>
        /// <param name="tokenCount">Reference to the current token count.</param>
        private static void ParseDoubleQuotedString(ReadOnlySpan<char> span, ref int pointer, Token[] tokens, ref int tokenCount)
        {
            int start = pointer;
            pointer++;
            while (pointer < span.Length && span[pointer] != '"') pointer++;
            if (pointer < span.Length && span[pointer] == '"') pointer++;
            tokens[tokenCount++] = new Token { Kind = TokenKind.Error, StartPosition = start, EndPosition = pointer };
        }

        /// <summary>
        /// Determines whether a character is a digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character is '0'–'9'; otherwise, false.</returns>
        private static bool IsDigit(char c) => (uint)(c - '0') <= 9;

        /// <summary>
        /// Determines whether a character can start an identifier.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character is a letter or underscore; otherwise, false.</returns>
        private static bool IsIdentifierStart(char c) => char.IsLetter(c) || c == '_';

        /// <summary>
        /// Determines whether a character can be part of an identifier.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character is a letter, digit, or underscore; otherwise, false.</returns>
        private static bool IsIdentifierPart(char c) => char.IsLetterOrDigit(c) || c == '_';
    }
}