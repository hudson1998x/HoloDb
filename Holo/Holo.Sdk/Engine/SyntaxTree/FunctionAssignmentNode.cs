namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents an assignment of a function call to a field in the syntax tree.
/// For example: <c>total = sum(values)</c>.
/// </summary>
public partial class FunctionAssignmentNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the field to which the function result is assigned.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Field;
    
    /// <summary>
    /// Gets or sets the function call being assigned to the field.
    /// This property is required and must be initialized.
    /// </summary>
    public required FunctionCallNode Function;
}