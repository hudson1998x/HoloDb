namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a field or property in the syntax tree, typically used within
/// objects, structs, or similar constructs.
/// </summary>
public partial class FieldNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the field.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Name;
}