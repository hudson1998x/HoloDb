using Holo.Sdk.Engine.SyntaxTree;

namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a type definition in the schema syntax.
/// Example: <c>type User { ... }</c>
/// </summary>
public sealed partial class TypeDefinitionNode : SyntaxNode
{
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    public required IdentifierNode TypeName { get; init; }

    /// <summary>
    /// Gets or sets the attributes applied to this type (e.g., @name, @description, @timestamps).
    /// </summary>
    public required NodeList Attributes { get; init; }

    /// <summary>
    /// Gets or sets the field definitions within this type.
    /// </summary>
    public required NodeList Fields { get; init; }

    public override TResult Accept<TResult>(IVisitor<TResult> visitor) => visitor.VisitTypeDefinitionNode(this);
    public override void Accept(IVisitor visitor) => visitor.VisitTypeDefinitionNode(this);
}
