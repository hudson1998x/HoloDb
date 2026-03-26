using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a single function parameter of the form:
    /// <c>$paramName: Type</c>.
    /// The type is required.
    /// Example: <c>$userId: int</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FunctionParameterNode"/>
    /// containing the parameter name and type.
    /// </returns>
    public static Production FunctionParameter()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Dollar sign prefix
                Production.TokenIs(TokenKind.DollarSign, _ => new EmptyNode()),

                // Parameter name
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("name"),

                // Colon separator  
                Production.TokenIs(TokenKind.Colon, _ => new EmptyNode()),

                // Parameter type
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("type")
            },
            captured =>
            {
                return new FunctionParameterNode
                {
                    Name = (IdentifierNode)captured["name"],
                    Type = (IdentifierNode)captured["type"],
                    DefaultValue = null
                };
            }
        );
    }
}