using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production AliasedField()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.Lazy(() => BaseField()).As("field"),  // Use BaseField, not Field!
            
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode() { Value = t }).As("as"),
            
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode() { Value = t }).As("alias")
            },
            captured =>
            {
                return new AliasedNode
                {
                    Left = captured["field"],
                    Alias = (IdentifierNode)captured["alias"]
                };
            }
        );
    }
}