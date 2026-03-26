using System.Text;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a syntax node with an alias.
/// Typically used to rename or label a sub-expression in a query or expression tree.
/// </summary>
public partial class AliasedNode
{
    /// <summary>
    /// Recursively prints a debug representation of this <see cref="AliasedNode"/>,
    /// including its <see cref="Left"/> expression and <see cref="Alias"/>,
    /// into the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="StringBuilder"/> to append the debug output to.
    /// </param>
    /// <param name="source">
    /// The original source text as a <see cref="ReadOnlySpan{T}"/>.
    /// Child nodes may use this span to display their corresponding text segments.
    /// </param>
    /// <param name="tabIndent">
    /// The indentation level (in multiples of 4 spaces) for formatting the debug output.
    /// Default is 0.
    /// </param>
    public override void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);

        builder.AppendLine($"{indent}AliasedNode {{");

        // Print the left-hand expression
        builder.AppendLine($"{indent}    Left {{");
        Left.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        // Print the alias
        builder.AppendLine($"{indent}    Alias {{");
        Alias.DebugPrint(builder, source, tabIndent + 2);
        builder.AppendLine($"{indent}    }}\n");

        builder.AppendLine($"{indent}}}");
    }
}