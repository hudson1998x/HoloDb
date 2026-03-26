using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

public static partial class Grammar
{
    public static Production FilterList()
    {
        return Production.DelimitedList(
            Production.Choice(
                FilterGroup(),
                FilterRule()
            ),
            TokenKind.Comma,
            nodes =>
            {
                // Multiple rules separated by commas are implicitly AND-ed
                return new FilterListNode
                {
                    Rules = new NodeList(nodes)
                };
            }
        );
    }
}