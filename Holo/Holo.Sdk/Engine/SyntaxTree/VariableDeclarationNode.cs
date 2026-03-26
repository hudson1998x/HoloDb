using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a variable declaration statement within a function body.
/// Example: <c>$roleId: int = getUserRole($userId)</c>.
/// </summary>
public partial class VariableDeclarationNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the variable (including the '$' prefix).
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the variable.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Type { get; set; }

    /// <summary>
    /// Gets or sets the expression used to initialize the variable.
    /// This property is required and must be initialized.
    /// </summary>
    public required SyntaxNode Value { get; set; }
}