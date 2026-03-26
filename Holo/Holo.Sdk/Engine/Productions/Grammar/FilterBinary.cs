using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterBinary()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.Lazy(() => FilterTerm()).As("left"),
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t }).As("op"),  // 'or', 'and'
                Production.Lazy(() => FilterExpression()).As("right")
            },
            captured =>
            {
                return new BinaryExpressionNode
                {
                    Left = captured["left"],
                    Operator = (IdentifierNode)captured["op"],
                    Right = captured["right"]
                };
            }
        );
    }
}