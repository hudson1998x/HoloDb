
using Holo.Sdk.Engine.Lexer;

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
        subQuery(
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
        subQuery( 
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