using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a literal value in the syntax tree (e.g., number, string, or boolean).
/// </summary>
public partial class LiteralNode
{
    /// <summary>
    /// Prints a debug representation of this <see cref="LiteralNode"/> to the provided <see cref="StringBuilder"/>.
    /// Includes detailed information about the value kind, positions in the source, and the actual literal text.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>. The literal's text is sliced from this span.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) to format the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}LiteralNode {{");

        builder.AppendLine($"{indent}    Value {{");
        builder.AppendLine($"{indent}        Kind({Value.Kind}) StartPosition({Value.StartPosition}) EndPosition({Value.EndPosition})");
        builder.AppendLine($"{indent}        Value: '{source.Slice(Value.StartPosition, Value.Length)}'");
        builder.AppendLine($"{indent}    }}");

        builder.AppendLine($"{indent}}}");
    }
}