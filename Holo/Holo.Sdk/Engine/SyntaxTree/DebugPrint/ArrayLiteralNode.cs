using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an array literal in the syntax tree.
/// Contains multiple items that are themselves syntax nodes.
/// </summary>
public partial class ArrayLiteralNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="ArrayLiteralNode"/>
    /// and its <see cref="Items"/> into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this to display the text corresponding to each array item.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}ArrayLiteralNode {{");

        // Print items in the array
        builder.AppendLine($"{indent}    Items {{");
        Items.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}