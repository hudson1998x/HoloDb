using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a function call in the syntax tree, including the function name and its arguments.
/// </summary>
public partial class FunctionCallNode : SyntaxNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="FunctionCallNode"/>,
    /// including its function name and argument nodes, into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>. Used by child nodes to reference their spans.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) to format the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);
        builder.AppendLine($"{indent}FunctionCallNode {{");
        
        // Print function name node
        builder.AppendLine($"{indent}    FunctionName {{");
        FunctionName.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");
        
        // Print arguments
        builder.AppendLine($"{indent}    Arguments {{");
        foreach (var arg in Arguments.Nodes)
        {
            arg.DebugPrint(builder, source, tabIndent + 2);
        }
        builder.AppendLine($"{indent}    }}");
        
        builder.AppendLine($"{indent}}}");
    }
}