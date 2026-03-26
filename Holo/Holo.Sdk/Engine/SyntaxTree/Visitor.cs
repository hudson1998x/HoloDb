namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Generic base class for visitors that traverse the syntax tree and return values.
/// Provides default traversal behavior that visits all child nodes.
/// </summary>
/// <typeparam name="TResult">The type of result returned by visit methods.</typeparam>
/// <remarks>
/// <para>
/// This class implements the visitor pattern with a return value. Override specific Visit methods
/// to customize behavior for particular node types. The default implementation recursively visits
/// all child nodes but returns <c>default(TResult)</c>.
/// </para>
/// <para>
/// To create a visitor:
/// <list type="number">
/// <item>Extend <see cref="Visitor{TResult}"/></item>
/// <item>Override the Visit methods for nodes you care about</item>
/// <item>Call <c>base.Visit()</c> for child traversal, or implement custom traversal</item>
/// </list>
/// </para>
/// </remarks>
public abstract class Visitor<TResult> : IVisitor<TResult>
{
    /// <summary>
    /// Visits a syntax node by calling its Accept method.
    /// This dispatches to the appropriate typed visit method.
    /// </summary>
    /// <param name="node">The syntax node to visit.</param>
    /// <returns>The result from the typed visit method.</returns>
    public virtual TResult Visit(SyntaxNode node) => node.Accept(this);

    /// <summary>
    /// Visits a <see cref="QueryNode"/> and all its children.
    /// </summary>
    /// <param name="node">The query node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitQueryNode(QueryNode node)
    {
        foreach (var child in node.Children)
            Visit(child);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="NamedBlockNode"/> including its name and fields.
    /// </summary>
    /// <param name="node">The named block node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitNamedBlockNode(NamedBlockNode node)
    {
        Visit(node.Name);
        Visit(node.Fields);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FieldNode"/> including its name.
    /// </summary>
    /// <param name="node">The field node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFieldNode(FieldNode node)
    {
        Visit(node.Name);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FilterFieldNode"/> including its field and filter.
    /// </summary>
    /// <param name="node">The filter field node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFilterFieldNode(FilterFieldNode node)
    {
        Visit(node.Field);
        Visit(node.Filter);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FilterListNode"/> including its rules.
    /// </summary>
    /// <param name="node">The filter list node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFilterListNode(FilterListNode node)
    {
        Visit(node.Rules);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FilterRuleNode"/> including its operator and value.
    /// </summary>
    /// <param name="node">The filter rule node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFilterRuleNode(FilterRuleNode node)
    {
        Visit(node.Operator);
        Visit(node.Value);
        return default!;
    }

    /// <summary>
    /// Visits an <see cref="IdentifierNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The identifier node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitIdentifierNode(IdentifierNode node) => default!;

    /// <summary>
    /// Visits a <see cref="LiteralNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The literal node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitLiteralNode(LiteralNode node) => default!;

    /// <summary>
    /// Visits an <see cref="EmptyNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The empty node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitEmptyNode(EmptyNode node) => default!;

    /// <summary>
    /// Visits an <see cref="AliasedNode"/> including its left expression and alias.
    /// </summary>
    /// <param name="node">The aliased node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitAliasedNode(AliasedNode node)
    {
        Visit(node.Left);
        Visit(node.Alias);
        return default!;
    }

    /// <summary>
    /// Visits an <see cref="ArrayLiteralNode"/> including its items.
    /// </summary>
    /// <param name="node">The array literal node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitArrayLiteralNode(ArrayLiteralNode node)
    {
        Visit(node.Items);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="BinaryExpressionNode"/> including left, operator, and right.
    /// </summary>
    /// <param name="node">The binary expression node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitBinaryExpressionNode(BinaryExpressionNode node)
    {
        Visit(node.Left);
        Visit(node.Operator);
        Visit(node.Right);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FunctionCallNode"/> including its name and arguments.
    /// </summary>
    /// <param name="node">The function call node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFunctionCallNode(FunctionCallNode node)
    {
        Visit(node.FunctionName);
        Visit(node.Arguments);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FunctionAssignmentNode"/> including its field and function.
    /// </summary>
    /// <param name="node">The function assignment node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFunctionAssignmentNode(FunctionAssignmentNode node)
    {
        Visit(node.Field);
        Visit(node.Function);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FunctionDefinitionNode"/> including all its components.
    /// </summary>
    /// <param name="node">The function definition node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFunctionDefinitionNode(FunctionDefinitionNode node)
    {
        Visit(node.Name);
        Visit(node.Parameters);
        Visit(node.ReturnType);
        Visit(node.Body);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FunctionParameterNode"/> including name, type, and optional default value.
    /// </summary>
    /// <param name="node">The function parameter node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFunctionParameterNode(FunctionParameterNode node)
    {
        Visit(node.Name);
        Visit(node.Type);
        if (node.DefaultValue != null)
            Visit(node.DefaultValue);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FunctionInvocationNode"/> including its name and arguments.
    /// </summary>
    /// <param name="node">The function invocation node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFunctionInvocationNode(FunctionInvocationNode node)
    {
        Visit(node.FunctionName);
        Visit(node.Arguments);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="VariableDeclarationNode"/> including name, type, and value.
    /// </summary>
    /// <param name="node">The variable declaration node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitVariableDeclarationNode(VariableDeclarationNode node)
    {
        Visit(node.Name);
        Visit(node.Type);
        Visit(node.Value);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="WhereClauseNode"/> including its filters.
    /// </summary>
    /// <param name="node">The where clause node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitWhereClauseNode(WhereClauseNode node)
    {
        Visit(node.Filters);
        return default!;
    }

    /// <summary>
    /// Visits all nodes in a <see cref="NodeList"/>.
    /// </summary>
    /// <param name="node">The node list to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitNodeList(NodeList node)
    {
        foreach (var child in node.Nodes)
            Visit(child);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="TypeDefinitionNode"/> including its type name, attributes, and fields.
    /// </summary>
    /// <param name="node">The type definition node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitTypeDefinitionNode(TypeDefinitionNode node)
    {
        Visit(node.TypeName);
        Visit(node.Attributes);
        Visit(node.Fields);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FieldDefinitionNode"/> including its field name and properties.
    /// </summary>
    /// <param name="node">The field definition node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFieldDefinitionNode(FieldDefinitionNode node)
    {
        Visit(node.FieldName);
        Visit(node.Properties);
        return default!;
    }

    /// <summary>
    /// Visits a <see cref="FieldPropertyNode"/> including its property name and arguments.
    /// </summary>
    /// <param name="node">The field property node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitFieldPropertyNode(FieldPropertyNode node)
    {
        Visit(node.PropertyName);
        Visit(node.Arguments);
        return default!;
    }

    /// <summary>
    /// Visits an <see cref="AttributeNode"/> including its attribute name and arguments.
    /// </summary>
    /// <param name="node">The attribute node to visit.</param>
    /// <returns>Default value of <typeparamref name="TResult"/>.</returns>
    public virtual TResult VisitAttributeNode(AttributeNode node)
    {
        Visit(node.AttributeName);
        Visit(node.Arguments);
        return default!;
    }
}

/// <summary>
/// Base class for visitors that traverse the syntax tree without returning values.
/// Provides default traversal behavior that visits all child nodes.
/// </summary>
/// <remarks>
/// <para>
/// This class implements the visitor pattern for side-effect operations (no return value).
/// Override specific Visit methods to customize behavior for particular node types.
/// </para>
/// <para>
/// Use this base class when you need to:
/// <list type="bullet">
/// <item>Print or format the AST</item>
/// <item>Validate the AST structure</item>
/// <item>Collect information during traversal</item>
/// <item>Transform nodes in place</item>
/// </list>
/// </para>
/// </remarks>
public abstract class Visitor : IVisitor
{
    /// <summary>
    /// Visits a syntax node by calling its Accept method.
    /// This dispatches to the appropriate typed visit method.
    /// </summary>
    /// <param name="node">The syntax node to visit.</param>
    public virtual void Visit(SyntaxNode node) => node.Accept(this);

    /// <summary>
    /// Visits a <see cref="QueryNode"/> and all its children.
    /// </summary>
    /// <param name="node">The query node to visit.</param>
    public virtual void VisitQueryNode(QueryNode node)
    {
        foreach (var child in node.Children)
            Visit(child);
    }

    /// <summary>
    /// Visits a <see cref="NamedBlockNode"/> including its name and fields.
    /// </summary>
    /// <param name="node">The named block node to visit.</param>
    public virtual void VisitNamedBlockNode(NamedBlockNode node)
    {
        Visit(node.Name);
        Visit(node.Fields);
    }

    /// <summary>
    /// Visits a <see cref="FieldNode"/> including its name.
    /// </summary>
    /// <param name="node">The field node to visit.</param>
    public virtual void VisitFieldNode(FieldNode node)
    {
        Visit(node.Name);
    }

    /// <summary>
    /// Visits a <see cref="FilterFieldNode"/> including its field and filter.
    /// </summary>
    /// <param name="node">The filter field node to visit.</param>
    public virtual void VisitFilterFieldNode(FilterFieldNode node)
    {
        Visit(node.Field);
        Visit(node.Filter);
    }

    /// <summary>
    /// Visits a <see cref="FilterListNode"/> including its rules.
    /// </summary>
    /// <param name="node">The filter list node to visit.</param>
    public virtual void VisitFilterListNode(FilterListNode node)
    {
        Visit(node.Rules);
    }

    /// <summary>
    /// Visits a <see cref="FilterRuleNode"/> including its operator and value.
    /// </summary>
    /// <param name="node">The filter rule node to visit.</param>
    public virtual void VisitFilterRuleNode(FilterRuleNode node)
    {
        Visit(node.Operator);
        Visit(node.Value);
    }

    /// <summary>
    /// Visits an <see cref="IdentifierNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The identifier node to visit.</param>
    public virtual void VisitIdentifierNode(IdentifierNode node) { }

    /// <summary>
    /// Visits a <see cref="LiteralNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The literal node to visit.</param>
    public virtual void VisitLiteralNode(LiteralNode node) { }

    /// <summary>
    /// Visits an <see cref="EmptyNode"/>. Leaf node - no children to visit.
    /// </summary>
    /// <param name="node">The empty node to visit.</param>
    public virtual void VisitEmptyNode(EmptyNode node) { }

    /// <summary>
    /// Visits an <see cref="AliasedNode"/> including its left expression and alias.
    /// </summary>
    /// <param name="node">The aliased node to visit.</param>
    public virtual void VisitAliasedNode(AliasedNode node)
    {
        Visit(node.Left);
        Visit(node.Alias);
    }

    /// <summary>
    /// Visits an <see cref="ArrayLiteralNode"/> including its items.
    /// </summary>
    /// <param name="node">The array literal node to visit.</param>
    public virtual void VisitArrayLiteralNode(ArrayLiteralNode node)
    {
        Visit(node.Items);
    }

    /// <summary>
    /// Visits a <see cref="BinaryExpressionNode"/> including left, operator, and right.
    /// </summary>
    /// <param name="node">The binary expression node to visit.</param>
    public virtual void VisitBinaryExpressionNode(BinaryExpressionNode node)
    {
        Visit(node.Left);
        Visit(node.Operator);
        Visit(node.Right);
    }

    /// <summary>
    /// Visits a <see cref="FunctionCallNode"/> including its name and arguments.
    /// </summary>
    /// <param name="node">The function call node to visit.</param>
    public virtual void VisitFunctionCallNode(FunctionCallNode node)
    {
        Visit(node.FunctionName);
        Visit(node.Arguments);
    }

    /// <summary>
    /// Visits a <see cref="FunctionAssignmentNode"/> including its field and function.
    /// </summary>
    /// <param name="node">The function assignment node to visit.</param>
    public virtual void VisitFunctionAssignmentNode(FunctionAssignmentNode node)
    {
        Visit(node.Field);
        Visit(node.Function);
    }

    /// <summary>
    /// Visits a <see cref="FunctionDefinitionNode"/> including all its components.
    /// </summary>
    /// <param name="node">The function definition node to visit.</param>
    public virtual void VisitFunctionDefinitionNode(FunctionDefinitionNode node)
    {
        Visit(node.Name);
        Visit(node.Parameters);
        Visit(node.ReturnType);
        Visit(node.Body);
    }

    /// <summary>
    /// Visits a <see cref="FunctionParameterNode"/> including name, type, and optional default value.
    /// </summary>
    /// <param name="node">The function parameter node to visit.</param>
    public virtual void VisitFunctionParameterNode(FunctionParameterNode node)
    {
        Visit(node.Name);
        Visit(node.Type);
        if (node.DefaultValue != null)
            Visit(node.DefaultValue);
    }

    /// <summary>
    /// Visits a <see cref="FunctionInvocationNode"/> including its name and arguments.
    /// </summary>
    /// <param name="node">The function invocation node to visit.</param>
    public virtual void VisitFunctionInvocationNode(FunctionInvocationNode node)
    {
        Visit(node.FunctionName);
        Visit(node.Arguments);
    }

    /// <summary>
    /// Visits a <see cref="VariableDeclarationNode"/> including name, type, and value.
    /// </summary>
    /// <param name="node">The variable declaration node to visit.</param>
    public virtual void VisitVariableDeclarationNode(VariableDeclarationNode node)
    {
        Visit(node.Name);
        Visit(node.Type);
        Visit(node.Value);
    }

    /// <summary>
    /// Visits a <see cref="WhereClauseNode"/> including its filters.
    /// </summary>
    /// <param name="node">The where clause node to visit.</param>
    public virtual void VisitWhereClauseNode(WhereClauseNode node)
    {
        Visit(node.Filters);
    }

    /// <summary>
    /// Visits all nodes in a <see cref="NodeList"/>.
    /// </summary>
    /// <param name="node">The node list to visit.</param>
    public virtual void VisitNodeList(NodeList node)
    {
        foreach (var child in node.Nodes)
            Visit(child);
    }

    /// <summary>
    /// Visits a <see cref="TypeDefinitionNode"/> including its type name, attributes, and fields.
    /// </summary>
    /// <param name="node">The type definition node to visit.</param>
    public virtual void VisitTypeDefinitionNode(TypeDefinitionNode node)
    {
        Visit(node.TypeName);
        Visit(node.Attributes);
        Visit(node.Fields);
    }

    /// <summary>
    /// Visits a <see cref="FieldDefinitionNode"/> including its field name and properties.
    /// </summary>
    /// <param name="node">The field definition node to visit.</param>
    public virtual void VisitFieldDefinitionNode(FieldDefinitionNode node)
    {
        Visit(node.FieldName);
        Visit(node.Properties);
    }

    /// <summary>
    /// Visits a <see cref="FieldPropertyNode"/> including its property name and arguments.
    /// </summary>
    /// <param name="node">The field property node to visit.</param>
    public virtual void VisitFieldPropertyNode(FieldPropertyNode node)
    {
        Visit(node.PropertyName);
        Visit(node.Arguments);
    }

    /// <summary>
    /// Visits an <see cref="AttributeNode"/> including its attribute name and arguments.
    /// </summary>
    /// <param name="node">The attribute node to visit.</param>
    public virtual void VisitAttributeNode(AttributeNode node)
    {
        Visit(node.AttributeName);
        Visit(node.Arguments);
    }
}
