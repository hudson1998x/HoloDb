using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a list of filter rules in a <see cref="WhereClauseNode"/>.
/// </summary>
public partial class FilterListNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="FilterListNode"/>
    /// and its contained rules into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this to show the text corresponding to each rule.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}FilterListNode {{");

        // Print contained rules
        builder.AppendLine($"{indent}    Rules {{");
        Rules.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}