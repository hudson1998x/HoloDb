
using System.Text;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.Productions;

var source = (
    """
    User { 
        id, 
        name, 
        email, 
        creatorUser { 
            id, 
            name, 
            avatar 
        }, 
        projects { 
            id, 
            title, 
            status 
        }, 
        userCount subQuery(
            User {
                 count(id) 
            }, 
            where {
                id {
                    above 20, 
                    below 40
                }
            }
        ),
        completedOrders subQuery( 
            Orders {
                count(id) as total
            },
            where {
                status {
                    in ['completed', 'dispatched']
                }
            }
        ) 
    }
    """);
    
var tokens = QueryLexer.Parse(source);

foreach (var token in tokens)
{
    Console.WriteLine($"Token ({token.Kind}, Start: {token.StartPosition}, End: {token.EndPosition}, Value: '{source.Substring(token.StartPosition, token.Length)}')");
}

var node = Parser.Parse(tokens, source);
var builder = new StringBuilder();

node.DebugPrint(builder, source);

Console.WriteLine("====================================================");
Console.WriteLine("Tree preview");
Console.WriteLine("====================================================");
Console.WriteLine(builder.ToString());