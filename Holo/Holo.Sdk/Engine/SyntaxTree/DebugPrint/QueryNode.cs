using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a complete query as a node in the syntax tree.
/// This node is the root of a query and can contain child nodes representing
/// components or clauses of the query.
/// </summary>
public partial class QueryNode
{
    /// <summary>
    /// Appends a human-readable, indented debug representation of this <see cref="QueryNode"/> 
    /// and its child nodes to the specified <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the debug output to.</param>
    /// <param name="source">
    /// The source text corresponding to this query.
    /// Provided as a <see cref="ReadOnlySpan{Char}"/> for efficient access without copying.
    /// </param>
    /// <param name="tabIndent">
    /// The current indentation level. Each level corresponds to 4 spaces.
    /// Defaults to 0 for the root node.
    /// </param>
    /// <remarks>
    /// This method recursively prints all child nodes in <see cref="Children"/>,
    /// adding commas and newlines between siblings for readability.
    /// Useful for debugging the structure of the query represented by this node.
    /// </remarks>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        // Create an indentation string based on the current level
        var indent = new string(' ', tabIndent * 4);
        
        // Start the node block
        builder.AppendLine($"{indent}QueryNode {{");

        foreach (var child in Children)
        {
            // Recursively print the child node with increased indentation
            child.DebugPrint(builder, source, tabIndent + 1);
        }
        
        // Close the node block
        builder.AppendLine($"{indent}}}");
    }
}