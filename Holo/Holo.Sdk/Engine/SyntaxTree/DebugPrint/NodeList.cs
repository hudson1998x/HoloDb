using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a list of <see cref="SyntaxNode"/> objects in the syntax tree.
/// </summary>
public partial class NodeList
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="NodeList"/> 
    /// and its child nodes into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>. 
    /// Child nodes may use this to reference their corresponding text.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) to format the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}NodeList {{");

        for (int i = 0; i < Nodes.Count; i++)
        {
            builder.AppendLine($"{indent}    Node [{i}] {{");
            Nodes[i].DebugPrint(builder, source, tabIndent + 2);
            builder.AppendLine($"{indent}    }}\n");
        }

        builder.AppendLine($"{indent}}}");
    }
}