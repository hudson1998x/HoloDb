namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a field with an associated filter in the syntax tree.
/// Typically used for expressions like <c>field[filter]</c>.
/// </summary>
public class FilterFieldNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the field being filtered.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Field { get; set; }

    /// <summary>
    /// Gets or sets the filter applied to the field.
    /// This property is required and must be initialized.
    /// </summary>
    public required SyntaxNode Filter { get; set; }
}