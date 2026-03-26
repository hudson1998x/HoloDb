namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a list of filter rules in the syntax tree.
/// Typically used to group multiple filter expressions together.
/// </summary>
public class FilterListNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the collection of filter rules.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Rules { get; set; }
}