using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a type definition of the form:
    /// <c>type TypeName { attributes... fields... }</c>
    /// Example: <c>type User { @name('User') id { type(int) } }</c>
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="TypeDefinitionNode"/>
    /// containing the type name, attributes, and field definitions.
    /// </returns>
    public static Production TypeDefinition()
    {
        return Production.IsSequence(
            new Production[]
            {
                // 'type' keyword
                Production.TokenIs(TokenKind.KeywordType, _ => new EmptyNode()),

                // Type name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode
                    {
                        Value = t
                    })
                    .As("typeName"),

                // Opening brace
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),

                // Attributes and fields (mixed order)
                Production.ZeroOrMore(
                    Production.Choice(
                        Production.Lazy(() => Attribute()),
                        Production.Lazy(() => FieldDefinition())
                    )
                ).As("members"),

                // Closing brace
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                var members = (NodeList)captured["members"];
                var attributes = new List<SyntaxNode>();
                var fields = new List<SyntaxNode>();

                foreach (var member in members.Nodes)
                {
                    if (member is AttributeNode)
                        attributes.Add(member);
                    else if (member is FieldDefinitionNode)
                        fields.Add(member);
                }

                return new TypeDefinitionNode
                {
                    TypeName = (IdentifierNode)captured["typeName"],
                    Attributes = new NodeList(attributes),
                    Fields = new NodeList(fields)
                };
            }
        );
    }
}
