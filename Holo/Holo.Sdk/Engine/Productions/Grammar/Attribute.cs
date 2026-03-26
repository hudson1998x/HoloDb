using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses an attribute of the form:
    /// <c>@attributeName()</c> or <c>@attributeName(arg)</c> or <c>@attributeName(arg1, arg2)</c>
    /// Examples: <c>@name('User')</c>, <c>@description('This is the user entity')</c>, <c>@timestamps()</c>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="AttributeNode"/>
    /// containing the attribute name and its arguments.
    /// </returns>
    public static Production Attribute()
    {
        return Production.IsSequence(
            new Production[]
            {
                // @ symbol
                Production.TokenIs(TokenKind.At, _ => new EmptyNode()),

                // Attribute name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode
                    {
                        Value = t
                    })
                    .As("attributeName"),

                // Opening parenthesis
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                // Arguments (optional, comma-separated)
                Production.Optional(
                    Production.DelimitedList(
                        Production.Choice(
                            Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }),
                            Production.TokenIs(TokenKind.StringLiteral, t => new LiteralNode { Value = t }),
                            Production.TokenIs(TokenKind.NumberLiteral, t => new LiteralNode { Value = t }),
                            Production.TokenIs(TokenKind.BooleanLiteral, t => new LiteralNode { Value = t })
                        ),
                        TokenKind.Comma
                    )
                ).As("arguments"),

                // Closing parenthesis
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured =>
            {
                var args = captured["arguments"];
                NodeList argList;
                if (args == null || args is EmptyNode)
                    argList = new NodeList();
                else
                    argList = (NodeList)args;

                return new AttributeNode
                {
                    AttributeName = (IdentifierNode)captured["attributeName"],
                    Arguments = argList
                };
            }
        );
    }
}
