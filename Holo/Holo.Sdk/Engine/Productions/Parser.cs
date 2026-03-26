using Holo.Sdk.Engine.Exceptions;
using Holo.Sdk.Engine.Lexer;
using Holo.Sdk.Engine.Registries;
using Holo.Sdk.Engine.Schema;
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
                TokenKind.KeywordFunction => TryProductions(
                    [
                        Grammar.FunctionDefinition()
                    ],
                    ref context
                ),
                TokenKind.KeywordType => TryProductions(
                    [
                        Grammar.TypeDefinition()
                    ],
                    ref context
                ),
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
            
            // Handle function definitions: register and continue (don't add to query)
            if (node is FunctionDefinitionNode functionDef)
            {
                RegisterStoredFunction(functionDef, source);
                continue;
            }

            // Handle type definitions: generate struct and register, then add to root
            if (node is TypeDefinitionNode typeDef)
            {
                var visitor = new TypeBuilderVisitor(source);
                visitor.VisitTypeDefinitionNode(typeDef);
                
                // Register all generated types
                foreach (var kvp in visitor.GeneratedTypes)
                {
                    SchemaRegistry.Register(kvp.Value);
                }
            }
            
            // Successfully parsed a node, add it to root and continue
            rootNode.Children.Add(node);
        }
        
        return rootNode;
    }

    /// <summary>
    /// Registers a parsed function definition in the function registry.
    /// </summary>
    /// <param name="functionDef">The function definition node to register.</param>
    /// <param name="source">The source text for extracting token text.</param>
    private static void RegisterStoredFunction(FunctionDefinitionNode functionDef, in ReadOnlySpan<char> source)
    {
        var parameterNames = new List<string>();
        var parameterTypes = new Dictionary<string, Type>();
        
        if (functionDef.Parameters.Nodes != null)
        {
            foreach (var param in functionDef.Parameters.Nodes)
            {
                if (param is FunctionParameterNode paramNode)
                {
                    var paramName = paramNode.Name.Value.GetText(source);
                    parameterNames.Add(paramName);
                    // Type mapping would happen at runtime when types are resolved
                    parameterTypes[paramName] = typeof(object); // Placeholder until type resolution
                }
            }
        }
        
        var storedFunction = new StoredFunction
        {
            Name = functionDef.Name.Value.GetText(source),
            Parameters = [.. parameterNames],
            ParameterTypes = parameterTypes,
            BodyAST = functionDef,
            ReturnType = typeof(object) // Placeholder until type resolution
        };
        
        FunctionRegistry.Register(storedFunction);
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