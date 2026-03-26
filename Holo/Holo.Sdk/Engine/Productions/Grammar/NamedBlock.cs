using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a named block of fields of the form:
    /// <c>name { field1, field2, ... }</c>.
    /// Example: <c>creatorUser { id, name, status }</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="NamedBlockNode"/>
    /// containing the block name and its list of fields.
    /// </returns>
    public static Production NamedBlock()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Block name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode
                    {
                        Value = t
                    })
                    .As("name"),

                // Opening brace
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),

                // Block fields (comma-separated)
                Production.Lazy(() => Fields()).As("fields"),

                // Closing brace
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new NamedBlockNode
                {
                    Name = (IdentifierNode)captured["name"],
                    Fields = (NodeList) captured["fields"]
                };
            }
        );
    }
}