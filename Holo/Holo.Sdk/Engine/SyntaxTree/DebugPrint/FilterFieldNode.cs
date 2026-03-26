using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a field in a filter clause along with its associated filter.
/// Typically used inside a <see cref="WhereClauseNode"/> to apply conditions to specific fields.
/// </summary>
public partial class FilterFieldNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="FilterFieldNode"/>,
    /// including its <see cref="Field"/> and <see cref="Filter"/> nodes,
    /// into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this to display their corresponding text spans.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}FilterFieldNode {{");

        // Print the field node
        builder.AppendLine($"{indent}    Field {{");
        Field.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        // Print the filter node
        builder.AppendLine($"{indent}    Filter {{");
        Filter.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}