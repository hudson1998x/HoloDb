using System.Text;

namespace Holo.Sdk.Engine.Helpers;

/// <summary>
/// Defines a contract for objects that can produce a textual debug representation of themselves.
/// </summary>
public interface DebugPrint
{
    /// <summary>
    /// Appends a debug representation of the object to the provided <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the debug output to.</param>
    /// <param name="source">The source code span corresponding to this object, useful for context.</param>
    /// <param name="tabIndent">
    /// The indentation level (in tab stops) to apply when formatting the output.
    /// Default is 0.
    /// </param>
    void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0);
}