using Holo.Sdk.Engine.Exceptions;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a variable declaration statement of the form:
    /// <c>$variableName: Type = expression</c>.
    /// Example: <c>$roleId: int = getUserRole($userId)</c>.
    /// </summary>
    public static Production VariableDeclaration()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.DollarSign, _ => new EmptyNode()),
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("name"),
                Production.TokenIs(TokenKind.Colon, _ => new EmptyNode()),
                Production.Choice(
                    Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordInt, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordString, t => new IdentifierNode { Value = t })
                ).As("type"),
                Production.TokenIs(TokenKind.Equal, _ => new EmptyNode()),
                Production.Choice(
                    Production.Lazy(() => FunctionCall()),
                    Production.Lazy(() => FunctionInvocation()),
                    Production.Lazy(() => Literal())
                ).As("value")
            },
            captured => new VariableDeclarationNode
            {
                Name = (IdentifierNode)captured["name"],
                Type = (IdentifierNode)captured["type"],
                Value = (SyntaxNode)captured["value"]
            }
        );
    }

    /// <summary>
    /// Parses a variable reference (e.g., $userId).
    /// </summary>
    public static Production VariableReference()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.DollarSign, _ => new EmptyNode()).As("dollar"),
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("varName")
            },
            captured => captured["varName"]
        );
    }

    /// <summary>
    /// Parses an invocation of a user-defined stored function.
    /// </summary>
    public static Production FunctionInvocation()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("name"),
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),
                Production.DelimitedList(
                    Production.Choice(
                        Production.Lazy(() => VariableReference()),
                        Production.Lazy(() => Literal()),
                        Production.Lazy(() => NamedBlock())
                    ),
                    TokenKind.Comma,
                    nodes => new NodeList(nodes)
                ).As("args"),
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode())
            },
            captured => new FunctionInvocationNode
            {
                FunctionName = (IdentifierNode)captured["name"],
                Arguments = (NodeList)captured["args"]
            }
        );
    }

    /// <summary>
    /// Parses a complete function definition.
    /// </summary>
    public static Production FunctionDefinition()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.KeywordFunction, _ => new EmptyNode()),
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("name"),
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),
                
                // Parameters: zero or more comma-separated params
                Production.ZeroOrMore(
                    // Each param: optionally preceded by comma (except first)
                    Production.Choice(
                        // First param (no comma)
                        Production.Lazy(() => FunctionParameter()),
                        // Subsequent params (comma then param)
                        Production.IsSequence(
                            new Production[]
                            {
                                Production.TokenIs(TokenKind.Comma, _ => new EmptyNode()).As("comma"),
                                Production.Lazy(() => FunctionParameter()).As("param")
                            },
                            captured => captured["param"]
                        )
                    ),
                    nodes => new NodeList(nodes)
                ).As("parameters"),
                
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode()),
                Production.TokenIs(TokenKind.Colon, _ => new EmptyNode()),
                Production.Choice(
                    Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordInt, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordString, t => new IdentifierNode { Value = t })
                ).As("returnType"),
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),
                
                Production.OneOrMore(
                    Production.Choice(
                        Production.Lazy(() => VariableDeclaration()),
                        Production.Lazy(() => FunctionCall()),
                        Production.Lazy(() => FunctionInvocation()),
                        Production.Lazy(() => NamedBlock()),
                        Production.TokenIs(TokenKind.KeywordReturn, _ => 
                            throw new SyntaxErrorException(
                                new Token { Kind = TokenKind.KeywordReturn, Text = "return" },
                                "Bottom named blocks are assumed as the return value - do not use 'return'."))
                    ),
                    nodes => new NodeList(nodes)
                ).As("body"),
                
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured => new FunctionDefinitionNode
            {
                Name = (IdentifierNode)captured["name"],
                Parameters = (NodeList)captured["parameters"],
                ReturnType = (IdentifierNode)captured["returnType"],
                Body = (NodeList)captured["body"]
            }
        );
    }
}
