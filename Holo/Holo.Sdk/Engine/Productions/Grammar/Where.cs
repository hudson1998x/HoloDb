using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar 
{
    public static Production WhereClause()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.KeywordWhere, t => new IdentifierNode { Value = t }),
            
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),
            
                Production.Lazy(() => Fields()).As("filters"),  // Filter fields inside the where block
            
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new WhereClauseNode
                {
                    Filters = (NodeList)captured["filters"]
                };
            }
        );
    }
}