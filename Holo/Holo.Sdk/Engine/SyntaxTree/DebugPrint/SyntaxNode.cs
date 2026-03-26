using System.Text;
using Holo.Sdk.Engine.Helpers;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a base syntax node in the syntax tree.
/// This class does not expose any properties and can be used as an empty node
/// or as a base class for other more specific syntax node types.
/// </summary>
public partial class SyntaxNode : DebugPrint
{
    /// <summary>
    /// Appends a debug representation of this node to the provided <see cref="StringBuilder"/>.
    /// Since <see cref="SyntaxNode"/> has no properties, this simply prints an empty node block.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="source">The source code span corresponding to this node.</param>
    /// <param name="tabIndent">The indentation level (number of tab stops) to apply.</param>
    public void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0)
    {
        var indent = new string(' ', tabIndent * 4);
        
        builder.AppendLine($"{indent}SyntaxNode {{");
        builder.AppendLine($"{indent}}}");
    }
}