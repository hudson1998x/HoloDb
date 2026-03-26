using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a function assignment expression of the form:
    /// <c>targetField functionName(arg1, arg2, ...)</c>.
    /// Example: <c>userCount subQuery(creatorUser, status)</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FunctionAssignmentNode"/>
    /// containing the target field and the called function with its arguments.
    /// </returns>
    public static Production FunctionAssignment()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Target field being assigned
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("target"),

                // Function name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode()
                    {
                        Value = t
                    })
                    .As("functionName"),

                // Opening parenthesis
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                // Function arguments: either named blocks or fields, comma-separated
                Production.DelimitedList(
                    Production.Choice(
                        Production.Lazy(() => NamedBlock()),
                        Production.Lazy(() => Field())
                    ),
                    TokenKind.Comma,
                    nodes => new NodeList(nodes)
                ).As("args"),

                // Closing parenthesis
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured =>
            {
                return new FunctionAssignmentNode
                {
                    Field = (IdentifierNode)captured["target"],
                    Function = new FunctionCallNode()
                    {
                        FunctionName = (IdentifierNode)captured["functionName"],
                        Arguments = (NodeList)captured["args"]
                    }
                };
            }
        );
    }
}