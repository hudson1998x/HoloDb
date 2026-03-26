using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a binary expression in the syntax tree, consisting of a left-hand side,
/// an operator, and a right-hand side.
/// </summary>
public partial class BinaryExpressionNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="BinaryExpressionNode"/>,
    /// including its <see cref="Left"/>, <see cref="Operator"/>, and <see cref="Right"/> nodes,
    /// into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this span to show their corresponding text segments.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}BinaryExpressionNode {{");

        // Print the left-hand side expression
        builder.AppendLine($"{indent}    Left {{");
        Left.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        // Print the operator
        builder.AppendLine($"{indent}    Operator {{");
        Operator.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        // Print the right-hand side expression
        builder.AppendLine($"{indent}    Right {{");
        Right.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}