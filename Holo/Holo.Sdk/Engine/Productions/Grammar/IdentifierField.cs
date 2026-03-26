using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a simple identifier field.
    /// Example: <c>id</c>, <c>name</c>, <c>userCount</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FieldNode"/>
    /// containing the parsed <see cref="IdentifierNode"/> as its name.
    /// </returns>
    public static Production IdentifierField()
    {
        return Production.TokenIs(TokenKind.Identifier, t =>
        {
            return new IdentifierNode()  // Return IdentifierNode directly, not wrapped in FieldNode
            {
                Value = t
            };
        });
    }
}