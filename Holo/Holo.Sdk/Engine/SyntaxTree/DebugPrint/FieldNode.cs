using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a field in the syntax tree.
/// Typically used to identify a named element in queries or filters.
/// </summary>
public partial class FieldNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="FieldNode"/>,
    /// including its <see cref="Name"/> node, into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this to show the text corresponding to the field name.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);
        
        builder.AppendLine($"{indent}FieldNode {{");

        // Print Name node (first time)
        builder.AppendLine($"{indent}    Field {{");
        Name.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        // Print Name node again (second time, may be intentional)
        builder.AppendLine($"{indent}    Field {{");
        Name.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}