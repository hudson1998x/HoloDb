using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Partial static class providing grammar rules for the parser.
/// </summary>
public static partial class Grammar
{
    /// <summary>
    /// Parses a comma-separated list of fields.
    /// Example: <c>id, name, count(subQuery(...)) as total</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="NodeList"/> of parsed fields.
    /// </returns>
    public static Production Fields()
    {
        return Production.DelimitedList(
            Production.Lazy(() => Field()),
            TokenKind.Comma,
            nodes => new NodeList(nodes)
        );
    }
    
    /// <summary>
    /// Parses a single field, which can be either an aliased field or a base field.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="SyntaxNode"/> representing the parsed field.
    /// </returns>
    public static Production Field()
    {
        return Production.Choice(
            AliasedField(),        // e.g., count(id) as total
            BaseField()            // Everything else
        );
    }
    
    /// <summary>
    /// Parses the basic forms of fields, including filters, blocks, function calls, identifiers, and literals.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="SyntaxNode"/> for the parsed base field.
    /// </returns>
    public static Production BaseField()
    {
        return Production.Choice(
            FilterField(),         // e.g., id { above 20, below 40 }
            NamedBlock(),          // e.g., creatorUser { ... }
            FunctionAssignment(),  // e.g., userCount subQuery(...)
            FunctionCall(),        // e.g., count(id)
            IdentifierField(),     // e.g., id
            Literal()              // e.g., 20, 'completed', etc.
        );
    }

    /// <summary>
    /// Parses a filtered field with a syntax like <c>fieldName { filterExpression }</c>.
    /// </summary>
    /// <returns>
    /// A <see cref="Production"/> that returns a <see cref="FilterFieldNode"/>
    /// containing the field identifier and the parsed filter expression.
    /// </returns>
    public static Production FilterField()
    {
        return Production.IsSequence(
            new Production[]
            {
                // Field identifier
                Production.TokenIs(TokenKind.Identifier, t => new IdentifierNode { Value = t })
                    .As("field"),
            
                // Opening bracket for filter
                Production.TokenIs(TokenKind.LeftBracket, _ => new EmptyNode()),
            
                // Filter expression
                Production.Lazy(() => FilterExpression()).As("filter"),
            
                // Closing bracket
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