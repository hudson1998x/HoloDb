namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Represents a list of syntax nodes in the syntax tree.
/// Useful for grouping multiple child nodes under a single parent node.
/// </summary>
public class NodeList : SyntaxNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeList"/> class
    /// with the specified collection of nodes.
    /// </summary>
    /// <param name="nodes">The list of syntax nodes to include in this node list.</param>
    public NodeList(List<SyntaxNode> nodes)
    {
        Nodes = nodes;
    }

    /// <summary>
    /// Gets or sets the list of syntax nodes contained in this node list.
    /// </summary>
    public List<SyntaxNode> Nodes { get; set; }
}