using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a field definition of the form:
    /// <c>fieldName { property1(), property2(arg), ... }</c>
    /// Example: <c>id { type(int), default(auto_increment), primary() }</c>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FieldDefinitionNode"/>
    /// containing the field name and its properties.
    /// </returns>
    public static Production FieldDefinition()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Field name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode
                    {
                        Value = t
                    })
                    .As("fieldName"),

                // Opening brace
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),

                // Field properties (space-separated, no commas)
                Production.ZeroOrMore(
                    Production.Lazy(() => FieldProperty())
                ).As("properties"),

                // Closing brace
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new FieldDefinitionNode
                {
                    FieldName = (IdentifierNode)captured["fieldName"],
                    Properties = (NodeList)captured["properties"]
                };
            }
        );
    }

    /// <summary>
    /// Parses a field property of the form:
    /// <c>propertyName()</c> or <c>propertyName(arg)</c> or <c>propertyName(arg1, arg2)</c>
    /// Examples: <c>type(int)</c>, <c>default(auto_increment)</c>, <c>primary()</c>, <c>validate(notEmpty)</c>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FieldPropertyNode"/>
    /// containing the property name and its arguments.
    /// </returns>
    public static Production FieldProperty()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Property name
                Production.Choice(
                    Production.TokenIs(TokenKind.KeywordType, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordDefault, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordPrimary, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordUnique, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordSensitive, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordNullable, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.NullLiteral, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordComment, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.KeywordValidate, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t })
                ).As("propertyName"),

                // Opening parenthesis
                Production.TokenIs(TokenKind.LeftParenthesis, _ => new EmptyNode()),

                // Arguments (optional)
                Production.Optional(
                    Production.DelimitedList(
                        Production.Choice(
                            Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }),
                            Production.TokenIs(TokenKind.StringLiteral, t => new LiteralNode { Value = t }),
                            Production.TokenIs(TokenKind.NumberLiteral, t => new LiteralNode { Value = t }),
                            Production.TokenIs(TokenKind.BooleanLiteral, t => new LiteralNode { Value = t }),
                            Production.TokenIs(TokenKind.NullLiteral, t => new LiteralNode { Value = t }),
                            // Type keywords as arguments
                            Production.TokenIs(TokenKind.KeywordInt, t => new IdentifierNode { Value = t }),
                            Production.TokenIs(TokenKind.KeywordString, t => new IdentifierNode { Value = t }),
                            Production.TokenIs(TokenKind.KeywordAutoIncrement, t => new IdentifierNode { Value = t })
                        ),
                        TokenKind.Comma
                    )
                ).As("arguments"),

                // Closing parenthesis
                Production.TokenIs(TokenKind.RightParenthesis, _ => new EmptyNode()),
            },
            captured =>
            {
                var args = captured["arguments"];
                NodeList argList;
                if (args == null || args is EmptyNode)
                    argList = new NodeList();
                else
                    argList = (NodeList)args;

                return new FieldPropertyNode
                {
                    PropertyName = (IdentifierNode)captured["propertyName"],
                    Arguments = argList
                };
            }
        );
    }
}
