using Holo.Sdk.Engine.Lexer;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a function definition in the syntax tree.
/// Functions are defined at the top level of a query document and can be called
/// from within other functions or queries.
/// </summary>
public partial class FunctionDefinitionNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the function.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode Name { get; set; }

    /// <summary>
    /// Gets or sets the list of parameters for the function.
    /// Each parameter must have a type specified.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Parameters { get; set; }

    /// <summary>
    /// Gets or sets the return type of the function.
    /// This property is required and must be initialized.
    /// </summary>
    public required IdentifierNode ReturnType { get; set; }

    /// <summary>
    /// Gets or sets the body of the function, containing statements.
    /// The last <see cref="NamedBlockNode"/> in this list is assumed to be the return value.
    /// This property is required and must be initialized.
    /// </summary>
    public required NodeList Body { get; set; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitFunctionDefinitionNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitFunctionDefinitionNode(this);
}