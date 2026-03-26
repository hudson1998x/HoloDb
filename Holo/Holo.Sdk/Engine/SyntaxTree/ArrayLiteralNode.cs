namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an array literal in the syntax tree.
/// For example: <c>[1, 2, 3]</c>.
/// </summary>
public partial class ArrayLiteralNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the list of items contained in the array literal.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Items { get; set; }
}