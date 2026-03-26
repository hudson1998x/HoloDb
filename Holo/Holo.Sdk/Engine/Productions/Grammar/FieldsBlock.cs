using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production Fields()
    {
        return Production.DelimitedList(
            Production.Lazy(() => Field()),
            TokenKind.Comma,
            nodes => new NodeList(nodes)
        );
    }
    
    public static Production Field()
    {
        return Production.Choice(
            AliasedField(),        // count(id) as total
            BaseField()            // Everything else
        );
    }
    
    public static Production BaseField()
    {
        return Production.Choice(
            FilterField(),         // id { above 20, below 40 }
            NamedBlock(),          // creatorUser { ... }
            FunctionAssignment(),  // userCount subQuery(...)
            FunctionCall(),        // count(id)
            IdentifierField(),     // id
            Literal()              // 20, 'completed', etc.
        );
    }

    public static Production FilterField()
    {
        return Production.IsSequence(
            new Production[]
            {
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t })
                    .As("field"),
            
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),
            
                Production.Lazy(() => FilterExpression()).As("filter"),
            
                Production.TokenIs(TokenKind.RightBracket, _ => new EmptyNode())
            },
            captured =>
            {
                return new FilterFieldNode
                {
                    Field = (IdentifierNode)captured["field"],
                    Filter = captured["filter"]
                };
            }
        );
    }
}