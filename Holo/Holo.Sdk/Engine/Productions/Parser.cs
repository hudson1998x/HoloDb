using Holo.Sdk.Engine.Exceptions;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.Productions;

/// <summary>
/// Provides parsing functionality to convert a sequence of tokens into a syntax tree.
/// </summary>
public static class Parser
{
    /// <summary>
    /// Parses the given tokens and source text into a <see cref="SyntaxNode"/> representing the root of the syntax tree.
    /// </summary>
    /// <param name="tokens">The token stream to parse.</param>
    /// <param name="source">The original source text for error reporting.</param>
    /// <returns>A <see cref="SyntaxNode"/> representing the root of the syntax tree.</returns>
    /// <exception cref="SyntaxErrorException">
    /// Thrown when the parser encounters an unexpected token or cannot match any production.
    /// </exception>
    public static SyntaxNode Parse(in ReadOnlySpan<Token> tokens, in ReadOnlySpan<char> source)
    {
        var context = new ParserContext(tokens);
        var rootNode = new QueryNode();
        
        while (!context.IsAtEnd)
        {
            var node = context.Peek()?.Kind switch
            {
                TokenKind.Identifier => TryProductions(
                    [ 
                        Grammar.NamedBlock(),
                        Grammar.FunctionAssignment()
                    ], 
                    ref context
                ),
                TokenKind.BooleanLiteral or TokenKind.NullLiteral or TokenKind.StringLiteral or TokenKind.NumberLiteral => 
                    TryProductions(
                        [
                            Grammar.Literal()
                        ],
                        ref context
                    ),
                _ => null
            };
            
            if (node == null)
            {
                // Use the furthest failed position for better error reporting
                var errorToken = context.MaxFailedPosition < tokens.Length 
                    ? tokens[context.MaxFailedPosition] 
                    : context.Peek()!.Value;
                throw new SyntaxErrorException(errorToken, source);
            }
            
            // Successfully parsed a node, add it to root and continue
            rootNode.Children.Add(node);
        }
        
        return rootNode;
    }
    
    /// <summary>
    /// Attempts to parse the input using a list of productions.
    /// Returns the first successfully parsed node or <c>null</c> if all productions fail.
    /// </summary>
    /// <param name="productions">An array of productions to try.</param>
    /// <param name="context">The parser context, passed by reference for state updates.</param>
    /// <returns>The successfully parsed <see cref="SyntaxNode"/> or <c>null</c> if none succeed.</returns>
    private static SyntaxNode? TryProductions(Production[] productions, ref ParserContext context)
    {
        var startPos = context.Position;

        foreach (var production in productions)
        {
            var result = production.Parse(ref context);
            if (result != null)
                return result;

            context.Rewind(startPos);
        }

        // Update max failed position when all productions fail
        context.MaxFailedPosition = Math.Max(context.MaxFailedPosition, context.Position);
        
        return null;
    }
}