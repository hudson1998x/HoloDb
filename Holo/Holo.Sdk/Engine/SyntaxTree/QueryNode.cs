namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a query in the syntax tree, which can contain multiple child nodes.
/// </summary>
public class QueryNode : SyntaxNode
{
    /// <summary>
    /// Gets the list of child nodes contained in this query.
    /// Initialized to an empty list.
    /// </summary>
    public readonly List<SyntaxNode> Children = new List<SyntaxNode>();
}