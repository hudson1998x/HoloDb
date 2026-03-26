using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterRule()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.Choice(
                    Production.TokenIs(TokenKind.KeywordIn, t => new IdentifierNode { Value = t }),
                    Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t })
                ).As("operator"),
            
                Production.Choice(
                    ArrayLiteral(),      // ['completed', 'dispatched']
                    Production.Lazy(() => Literal())  // 20, 'string', etc.
                ).As("value")
            },
            captured =>
            {
                return new FilterRuleNode
                {
                    Operator = (IdentifierNode)captured["operator"],
                    Value = captured["value"]
                };
            }
        );
    }
}