using System.Text;

namespace Holo.Sdk.Engine.Helpers;

public interface DebugPrint
{
    public void DebugPrint(StringBuilder builder, in ReadOnlySpan<char> source, int tabIndent = 0);
}