using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a function call expression of the form:
    /// <c>functionName(arg1, arg2, ...)</c>.
    /// Arguments can be fields, named blocks, or other expressions.
    /// Example: <c>count(id)</c> or <c>subQuery(user { status }, 20)</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FunctionCallNode"/>
    /// containing the function name and its parsed arguments.
    /// </returns>
    public static Production FunctionCall()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Function name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("name"),

                // Opening parenthesis
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                // Arguments: either NamedBlock or Field, comma-separated
                Production.DelimitedList(
                    Production.Choice(
                        Production.Lazy(() => NamedBlock()),
                        Production.Lazy(() => Field()) // recursion: arguments can be expressions
                    ),
                    TokenKind.Comma,
                    nodes =>
                    {
                        var args = new NodeList(nodes);
                        return args;
                    }
                ).As("args"),

                // Closing parenthesis
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured =>
            {
                return new FunctionCallNode()
                {
                    FunctionName = (IdentifierNode) captured["name"],
                    Arguments = (NodeList) captured["args"]
                };
            }
        );
    }
}