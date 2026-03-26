using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an assignment of a function to a field in the syntax tree.
/// </summary>
public partial class FunctionAssignmentNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="FunctionAssignmentNode"/>,
    /// including the field being assigned and the function being assigned to it,
    /// into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes can reference this span to show their corresponding text.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) to format the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}FunctionAssignmentNode {{");

        // Print the target field node
        builder.AppendLine($"{indent}    Field {{");
        Field.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}");

        // Print the function node
        builder.AppendLine($"{indent}    Function {{");
        Function.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}");

        builder.AppendLine($"{indent}}}");
    }
}