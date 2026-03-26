using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a "where" clause in a syntax tree for filtering data.
/// </summary>
public partial class WhereClauseNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="WhereClauseNode"/> 
    /// and its child filters into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>. 
    /// This can be used by child nodes to reference their span of text.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) to apply for formatting the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);
        builder.AppendLine($"{indent}{GetType().Name}{{");
        
        builder.AppendLine($"{indent}    Filters {{");

        foreach (var filter in Filters.Nodes)
        {
            filter.DebugPrint(builder, source, tabIndent + 2);
        }
        
        builder.AppendLine($"{indent}    }}");
        
        builder.AppendLine($"{indent}}}");
    }
}