using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a parameter in a function definition.
/// Parameters must have a type specified; default values are optional.
/// Example: <c>$userId: int</c> or <c>$limit: int = 10</c>.
/// </summary>
public partial class FunctionParameterNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the parameter (including the '$' prefix).
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the parameter.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Type { get; set; }

    /// <summary>
    /// Gets or sets the default value for the parameter, if provided.
    /// This property is null if no default value is specified.
    /// </summary>
    public SyntaxNode? DefaultValue { get; set; }
}