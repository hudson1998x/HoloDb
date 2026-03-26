using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.Lexer.Tests;

namespace Holo.Tests.Engine.Lexer
{
    /// <summary>
    /// Unit tests for <see cref="QueryLexer"/>, covering symbols, keywords, numbers, identifiers, strings, and complex queries.
    /// </summary>
    public class QueryLexerTests
    {
        // -------------------------
        // Symbols
        // -------------------------

        /// <summary>
        /// Ensures all individual symbol characters are correctly tokenized.
        /// </summary>
        [Fact]
        public void Parses_All_Symbols()
        {
            var input = "[](){}<>,!=$:";
            var tokens = LexerTestHelper.Lex(input);

            var expected = new[]
            {
                TokenKind.LeftSquareBracket,
                TokenKind.RightSquareBracket,
                TokenKind.LeftParenthesis,
                TokenKind.RightParenthesis,
                TokenKind.LeftBracket,
                TokenKind.RightBracket,
                TokenKind.LessThan,
                TokenKind.MoreThan,
                TokenKind.Comma,
                TokenKind.KeywordNot,
                TokenKind.Equal,
                TokenKind.DollarSign,
                TokenKind.Colon
            };

            Assert.Equal(expected.Length, tokens.Length);

            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], tokens[i].Kind);
        }

        // -------------------------
        // Keywords
        // -------------------------

        /// <summary>
        /// Ensures exact keywords are correctly tokenized to their respective <see cref="TokenKind"/>.
        /// </summary>
        /// <param name="input">The keyword input string.</param>
        /// <param name="expected">The expected <see cref="TokenKind"/>.</param>
        [Theory]
        [InlineData("is", TokenKind.KeywordIs)]
        [InlineData("not", TokenKind.KeywordNot)]
        [InlineData("null", TokenKind.NullLiteral)]
        [InlineData("true", TokenKind.BooleanLiteral)]
        [InlineData("false", TokenKind.BooleanLiteral)]
        [InlineData("contains", TokenKind.KeywordContains)]
        [InlineData("startsWith", TokenKind.KeywordStartsWith)]
        [InlineData("endsWith", TokenKind.KeywordEndsWith)]
        [InlineData("within", TokenKind.KeywordWithin)]
        [InlineData("function", TokenKind.KeywordFunction)]
        [InlineData("return", TokenKind.KeywordReturn)]
        public void Parses_Exact_Keywords(string input, TokenKind expected)
        {
            var tokens = LexerTestHelper.Lex(input);
            Assert.Single(tokens);
            Assert.Equal(expected, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures keywords do not match when part of an identifier.
        /// </summary>
        /// <param name="input">The identifier containing a keyword.</param>
        [Theory]
        [InlineData("island")]
        [InlineData("trueValue")]
        [InlineData("containsX")]
        public void Keywords_Dont_Match_Inside_Identifiers(string input)
        {
            var tokens = LexerTestHelper.Lex(input);
            Assert.Single(tokens);
            Assert.Equal(TokenKind.Identifier, tokens[0].Kind);
        }

        // -------------------------
        // Numbers
        // -------------------------

        /// <summary>
        /// Ensures integer numbers are correctly tokenized.
        /// </summary>
        /// <param name="input">The numeric input.</param>
        [Theory]
        [InlineData("0")]
        [InlineData("123")]
        [InlineData("99999")]
        public void Parses_Integer_Numbers(string input)
        {
            var tokens = LexerTestHelper.Lex(input);
            Assert.Single(tokens);
            Assert.Equal(TokenKind.NumberLiteral, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures floating-point numbers are correctly tokenized.
        /// </summary>
        /// <param name="input">The floating-point input.</param>
        [Theory]
        [InlineData("1.0")]
        [InlineData("123.456")]
        [InlineData("0.001")]
        public void Parses_FloatingPoint_Numbers(string input)
        {
            var tokens = LexerTestHelper.Lex(input);
            Assert.Single(tokens);
            Assert.Equal(TokenKind.NumberLiteral, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures numbers followed immediately by identifiers are split correctly.
        /// </summary>
        [Fact]
        public void Splits_Number_And_Identifier()
        {
            var tokens = LexerTestHelper.Lex("123abc");
            Assert.Equal(2, tokens.Length);
            Assert.Equal(TokenKind.NumberLiteral, tokens[0].Kind);
            Assert.Equal(TokenKind.Identifier, tokens[1].Kind);
        }

        // -------------------------
        // Identifiers
        // -------------------------

        /// <summary>
        /// Ensures valid identifiers are correctly tokenized.
        /// </summary>
        /// <param name="input">The identifier input string.</param>
        [Theory]
        [InlineData("a")]
        [InlineData("_a")]
        [InlineData("abc123")]
        [InlineData("my_field")]
        public void Parses_Valid_Identifiers(string input)
        {
            var tokens = LexerTestHelper.Lex(input);
            Assert.Single(tokens);
            Assert.Equal(TokenKind.Identifier, tokens[0].Kind);
        }

        // -------------------------
        // Strings
        // -------------------------

        /// <summary>
        /// Ensures single-quoted strings are parsed correctly.
        /// </summary>
        [Fact]
        public void Parses_SingleQuoted_String()
        {
            var tokens = LexerTestHelper.Lex("'hello world'");
            Assert.Single(tokens);
            Assert.Equal(TokenKind.StringLiteral, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures unterminated single-quoted strings produce an error token.
        /// </summary>
        [Fact]
        public void Unterminated_SingleQuoted_String_Is_Error()
        {
            var tokens = LexerTestHelper.Lex("'hello");
            Assert.Single(tokens);
            Assert.Equal(TokenKind.Error, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures empty single-quoted strings are parsed as valid string literals.
        /// </summary>
        [Fact]
        public void Empty_SingleQuoted_String()
        {
            var tokens = LexerTestHelper.Lex("''");
            Assert.Single(tokens);
            Assert.Equal(TokenKind.StringLiteral, tokens[0].Kind);
        }

        /// <summary>
        /// Ensures double-quoted strings always produce an error token.
        /// </summary>
        [Fact]
        public void DoubleQuoted_String_Is_Error()
        {
            var tokens = LexerTestHelper.Lex("\"hello\"");
            Assert.Single(tokens);
            Assert.Equal(TokenKind.Error, tokens[0].Kind);
        }

        // -------------------------
        // Mixed Queries
        // -------------------------

        /// <summary>
        /// Tests a mixed query containing identifiers, keywords, strings, numbers, and operators.
        /// </summary>
        [Fact]
        public void Parses_Mixed_Query()
        {
            var input = "name startsWith 'Jo' and age > 18";
            var tokens = LexerTestHelper.Lex(input);

            var expected = new[]
            {
                TokenKind.Identifier,
                TokenKind.KeywordStartsWith,
                TokenKind.StringLiteral,
                TokenKind.Identifier, // "and" fallback
                TokenKind.Identifier, // "age"
                TokenKind.MoreThan,
                TokenKind.NumberLiteral
            };

            Assert.Equal(expected.Length, tokens.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], tokens[i].Kind);
        }

        /// <summary>
        /// Ensures nested structures like parentheses and commas are tokenized correctly.
        /// </summary>
        [Fact]
        public void Parses_Nested_Structures()
        {
            var input = "(a, b, c)";
            var tokens = LexerTestHelper.Lex(input);

            var expected = new[]
            {
                TokenKind.LeftParenthesis,
                TokenKind.Identifier,
                TokenKind.Comma,
                TokenKind.Identifier,
                TokenKind.Comma,
                TokenKind.Identifier,
                TokenKind.RightParenthesis
            };

            Assert.Equal(expected.Length, tokens.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], tokens[i].Kind);
        }

        // -------------------------
        // Whitespace
        // -------------------------

        /// <summary>
        /// Ensures that whitespace is skipped and does not produce tokens.
        /// </summary>
        [Fact]
        public void Skips_Whitespace()
        {
            var tokens = LexerTestHelper.Lex("   true   ");
            Assert.Single(tokens);
            Assert.Equal(TokenKind.BooleanLiteral, tokens[0].Kind);
        }

        // -------------------------
        // Unknown characters
        // -------------------------

        /// <summary>
        /// Ensures unknown characters are ignored and do not produce tokens.
        /// </summary>
        [Fact]
        public void Skips_Unknown_Characters()
        {
            var tokens = LexerTestHelper.Lex("#%^");
            Assert.Empty(tokens);
        }

        // -------------------------
        // Pointer and Span validation
        // -------------------------

        /// <summary>
        /// Validates that start and end positions of tokens are tracked correctly.
        /// </summary>
        [Fact]
        public void Tracks_Exact_StartEnd_Positions()
        {
            var tokens = LexerTestHelper.Lex("a is 10");

            LexerTestHelper.AssertToken(tokens[0], TokenKind.Identifier, 0, 1);
            LexerTestHelper.AssertToken(tokens[1], TokenKind.KeywordIs, 2, 4);
            LexerTestHelper.AssertToken(tokens[2], TokenKind.NumberLiteral, 5, 7);
        }

        // -------------------------
        // Robustness / Fuzz
        // -------------------------

        /// <summary>
        /// Ensures that random input strings do not cause exceptions in the lexer.
        /// </summary>
        [Fact]
        public void RandomInput_DoesNotThrow()
        {
            var random = new Random(42);

            for (int i = 0; i < 1000; i++)
            {
                var length = random.Next(1, 50);
                var chars = new char[length];
                for (int j = 0; j < length; j++)
                    chars[j] = (char)random.Next(32, 126);

                var input = new string(chars);
                var exception = Record.Exception(() => QueryLexer.Parse(input));
                Assert.Null(exception);
            }
        }
    }
}