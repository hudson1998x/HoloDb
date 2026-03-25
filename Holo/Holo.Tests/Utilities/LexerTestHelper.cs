using Xunit;

namespace Holo.Sdk.Engine.Lexer.Tests;

public static class LexerTestHelper
{
    public static Token[] Lex(string input)
        => QueryLexer.Parse(input.AsSpan());

    public static void AssertToken(
        Token token,
        TokenKind kind,
        int start,
        int end)
    {
        Assert.Equal(kind, token.Kind);
        Assert.Equal(start, token.StartPosition);
        Assert.Equal(end, token.EndPosition);
    }
}