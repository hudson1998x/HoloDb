using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production IdentifierField()
    {
        return Production.TokenIs(TokenKind.Identifier, t =>
        {
            return new FieldNode
            {
                Name = new IdentifierNode()
                {
                    Value = t
                }
            };
        });
    }
}