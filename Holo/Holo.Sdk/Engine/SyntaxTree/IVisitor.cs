namespace Holo.Sdk.Engine.SyntaxTree;

/// <summary>
/// Generic interface for visiting syntax tree nodes with a return value.
/// Implement this interface to create visitors that traverse the AST and produce results.
/// </summary>
/// <typeparam name="TResult">The type of result returned by visit methods.</typeparam>
/// <remarks>
/// <para>
/// The visitor pattern allows adding new operations to existing object structures without modifying those structures.
/// Implementations can override specific Visit methods to customize behavior for particular node types.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// public class MyVisitor : IVisitor&lt;int&gt;
/// {
///     public int Visit(SyntaxNode node) => node.Accept(this);
///     public int VisitIdentifierNode(IdentifierNode node) => 1;
///     // ... implement other Visit methods
/// }
/// </code>
/// </para>
/// </remarks>
public interface IVisitor<TResult>
{
    /// <summary>
    /// Visits a syntax node and dispatches to the appropriate typed visit method.
    /// </summary>
    /// <param name="node">The syntax node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult Visit(SyntaxNode node);

    /// <summary>
    /// Visits a <see cref="QueryNode"/> containing the root of a query.
    /// </summary>
    /// <param name="node">The query node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitQueryNode(QueryNode node);

    /// <summary>
    /// Visits a <see cref="NamedBlockNode"/> representing a named collection of fields.
    /// </summary>
    /// <param name="node">The named block node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitNamedBlockNode(NamedBlockNode node);

    /// <summary>
    /// Visits a <see cref="FieldNode"/> representing a field or property.
    /// </summary>
    /// <param name="node">The field node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFieldNode(FieldNode node);

    /// <summary>
    /// Visits a <see cref="FilterFieldNode"/> representing a field with an associated filter.
    /// </summary>
    /// <param name="node">The filter field node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFilterFieldNode(FilterFieldNode node);

    /// <summary>
    /// Visits a <see cref="FilterListNode"/> representing a list of filter rules.
    /// </summary>
    /// <param name="node">The filter list node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFilterListNode(FilterListNode node);

    /// <summary>
    /// Visits a <see cref="FilterRuleNode"/> representing a single filter rule with operator and value.
    /// </summary>
    /// <param name="node">The filter rule node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFilterRuleNode(FilterRuleNode node);

    /// <summary>
    /// Visits an <see cref="IdentifierNode"/> representing an identifier.
    /// </summary>
    /// <param name="node">The identifier node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitIdentifierNode(IdentifierNode node);

    /// <summary>
    /// Visits a <see cref="LiteralNode"/> representing a literal value (number, string, boolean).
    /// </summary>
    /// <param name="node">The literal node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitLiteralNode(LiteralNode node);

    /// <summary>
    /// Visits an <see cref="EmptyNode"/> representing an empty or placeholder node.
    /// </summary>
    /// <param name="node">The empty node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitEmptyNode(EmptyNode node);

    /// <summary>
    /// Visits an <see cref="AliasedNode"/> representing a node with an alias.
    /// </summary>
    /// <param name="node">The aliased node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitAliasedNode(AliasedNode node);

    /// <summary>
    /// Visits an <see cref="ArrayLiteralNode"/> representing an array literal.
    /// </summary>
    /// <param name="node">The array literal node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitArrayLiteralNode(ArrayLiteralNode node);

    /// <summary>
    /// Visits a <see cref="BinaryExpressionNode"/> representing a binary expression.
    /// </summary>
    /// <param name="node">The binary expression node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitBinaryExpressionNode(BinaryExpressionNode node);

    /// <summary>
    /// Visits a <see cref="FunctionCallNode"/> representing a built-in function call.
    /// </summary>
    /// <param name="node">The function call node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFunctionCallNode(FunctionCallNode node);

    /// <summary>
    /// Visits a <see cref="FunctionAssignmentNode"/> representing a function assignment.
    /// </summary>
    /// <param name="node">The function assignment node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFunctionAssignmentNode(FunctionAssignmentNode node);

    /// <summary>
    /// Visits a <see cref="FunctionDefinitionNode"/> representing a user-defined function.
    /// </summary>
    /// <param name="node">The function definition node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFunctionDefinitionNode(FunctionDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FunctionParameterNode"/> representing a function parameter.
    /// </summary>
    /// <param name="node">The function parameter node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFunctionParameterNode(FunctionParameterNode node);

    /// <summary>
    /// Visits a <see cref="FunctionInvocationNode"/> representing an invocation of a user-defined function.
    /// </summary>
    /// <param name="node">The function invocation node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFunctionInvocationNode(FunctionInvocationNode node);

    /// <summary>
    /// Visits a <see cref="VariableDeclarationNode"/> representing a variable declaration.
    /// </summary>
    /// <param name="node">The variable declaration node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitVariableDeclarationNode(VariableDeclarationNode node);

    /// <summary>
    /// Visits a <see cref="WhereClauseNode"/> representing a WHERE clause with filters.
    /// </summary>
    /// <param name="node">The where clause node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitWhereClauseNode(WhereClauseNode node);

    /// <summary>
    /// Visits a <see cref="NodeList"/> containing a list of syntax nodes.
    /// </summary>
    /// <param name="node">The node list to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitNodeList(NodeList node);

    /// <summary>
    /// Visits a <see cref="TypeDefinitionNode"/> representing a schema type definition.
    /// </summary>
    /// <param name="node">The type definition node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitTypeDefinitionNode(TypeDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FieldDefinitionNode"/> representing a field definition within a type.
    /// </summary>
    /// <param name="node">The field definition node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFieldDefinitionNode(FieldDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FieldPropertyNode"/> representing a field property (type, default, primary, etc.).
    /// </summary>
    /// <param name="node">The field property node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitFieldPropertyNode(FieldPropertyNode node);

    /// <summary>
    /// Visits an <see cref="AttributeNode"/> representing a schema attribute (e.g., @name, @description).
    /// </summary>
    /// <param name="node">The attribute node to visit.</param>
    /// <returns>The result of visiting the node.</returns>
    TResult VisitAttributeNode(AttributeNode node);
}

/// <summary>
/// Non-generic interface for visiting syntax tree nodes without a return value.
/// Implement this interface to create visitors that traverse the AST for side effects.
/// </summary>
/// <remarks>
/// <para>
/// This interface is useful for visitors that perform actions like printing, validation,
/// or transformation without needing to return a computed value.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// public class PrintVisitor : IVisitor
/// {
///     public void Visit(SyntaxNode node) => node.Accept(this);
///     public void VisitIdentifierNode(IdentifierNode node) => Console.WriteLine(node.Value);
///     // ... implement other Visit methods
/// }
/// </code>
/// </para>
/// </remarks>
public interface IVisitor
{
    /// <summary>
    /// Visits a syntax node and dispatches to the appropriate typed visit method.
    /// </summary>
    /// <param name="node">The syntax node to visit.</param>
    void Visit(SyntaxNode node);

    /// <summary>
    /// Visits a <see cref="QueryNode"/> containing the root of a query.
    /// </summary>
    /// <param name="node">The query node to visit.</param>
    void VisitQueryNode(QueryNode node);

    /// <summary>
    /// Visits a <see cref="NamedBlockNode"/> representing a named collection of fields.
    /// </summary>
    /// <param name="node">The named block node to visit.</param>
    void VisitNamedBlockNode(NamedBlockNode node);

    /// <summary>
    /// Visits a <see cref="FieldNode"/> representing a field or property.
    /// </summary>
    /// <param name="node">The field node to visit.</param>
    void VisitFieldNode(FieldNode node);

    /// <summary>
    /// Visits a <see cref="FilterFieldNode"/> representing a field with an associated filter.
    /// </summary>
    /// <param name="node">The filter field node to visit.</param>
    void VisitFilterFieldNode(FilterFieldNode node);

    /// <summary>
    /// Visits a <see cref="FilterListNode"/> representing a list of filter rules.
    /// </summary>
    /// <param name="node">The filter list node to visit.</param>
    void VisitFilterListNode(FilterListNode node);

    /// <summary>
    /// Visits a <see cref="FilterRuleNode"/> representing a single filter rule with operator and value.
    /// </summary>
    /// <param name="node">The filter rule node to visit.</param>
    void VisitFilterRuleNode(FilterRuleNode node);

    /// <summary>
    /// Visits an <see cref="IdentifierNode"/> representing an identifier.
    /// </summary>
    /// <param name="node">The identifier node to visit.</param>
    void VisitIdentifierNode(IdentifierNode node);

    /// <summary>
    /// Visits a <see cref="LiteralNode"/> representing a literal value (number, string, boolean).
    /// </summary>
    /// <param name="node">The literal node to visit.</param>
    void VisitLiteralNode(LiteralNode node);

    /// <summary>
    /// Visits an <see cref="EmptyNode"/> representing an empty or placeholder node.
    /// </summary>
    /// <param name="node">The empty node to visit.</param>
    void VisitEmptyNode(EmptyNode node);

    /// <summary>
    /// Visits an <see cref="AliasedNode"/> representing a node with an alias.
    /// </summary>
    /// <param name="node">The aliased node to visit.</param>
    void VisitAliasedNode(AliasedNode node);

    /// <summary>
    /// Visits an <see cref="ArrayLiteralNode"/> representing an array literal.
    /// </summary>
    /// <param name="node">The array literal node to visit.</param>
    void VisitArrayLiteralNode(ArrayLiteralNode node);

    /// <summary>
    /// Visits a <see cref="BinaryExpressionNode"/> representing a binary expression.
    /// </summary>
    /// <param name="node">The binary expression node to visit.</param>
    void VisitBinaryExpressionNode(BinaryExpressionNode node);

    /// <summary>
    /// Visits a <see cref="FunctionCallNode"/> representing a built-in function call.
    /// </summary>
    /// <param name="node">The function call node to visit.</param>
    void VisitFunctionCallNode(FunctionCallNode node);

    /// <summary>
    /// Visits a <see cref="FunctionAssignmentNode"/> representing a function assignment.
    /// </summary>
    /// <param name="node">The function assignment node to visit.</param>
    void VisitFunctionAssignmentNode(FunctionAssignmentNode node);

    /// <summary>
    /// Visits a <see cref="FunctionDefinitionNode"/> representing a user-defined function.
    /// </summary>
    /// <param name="node">The function definition node to visit.</param>
    void VisitFunctionDefinitionNode(FunctionDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FunctionParameterNode"/> representing a function parameter.
    /// </summary>
    /// <param name="node">The function parameter node to visit.</param>
    void VisitFunctionParameterNode(FunctionParameterNode node);

    /// <summary>
    /// Visits a <see cref="FunctionInvocationNode"/> representing an invocation of a user-defined function.
    /// </summary>
    /// <param name="node">The function invocation node to visit.</param>
    void VisitFunctionInvocationNode(FunctionInvocationNode node);

    /// <summary>
    /// Visits a <see cref="VariableDeclarationNode"/> representing a variable declaration.
    /// </summary>
    /// <param name="node">The variable declaration node to visit.</param>
    void VisitVariableDeclarationNode(VariableDeclarationNode node);

    /// <summary>
    /// Visits a <see cref="WhereClauseNode"/> representing a WHERE clause with filters.
    /// </summary>
    /// <param name="node">The where clause node to visit.</param>
    void VisitWhereClauseNode(WhereClauseNode node);

    /// <summary>
    /// Visits a <see cref="NodeList"/> containing a list of syntax nodes.
    /// </summary>
    /// <param name="node">The node list to visit.</param>
    void VisitNodeList(NodeList node);

    /// <summary>
    /// Visits a <see cref="TypeDefinitionNode"/> representing a schema type definition.
    /// </summary>
    /// <param name="node">The type definition node to visit.</param>
    void VisitTypeDefinitionNode(TypeDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FieldDefinitionNode"/> representing a field definition within a type.
    /// </summary>
    /// <param name="node">The field definition node to visit.</param>
    void VisitFieldDefinitionNode(FieldDefinitionNode node);

    /// <summary>
    /// Visits a <see cref="FieldPropertyNode"/> representing a field property (type, default, primary, etc.).
    /// </summary>
    /// <param name="node">The field property node to visit.</param>
    void VisitFieldPropertyNode(FieldPropertyNode node);

    /// <summary>
    /// Visits an <see cref="AttributeNode"/> representing a schema attribute (e.g., @name, @description).
    /// </summary>
    /// <param name="node">The attribute node to visit.</param>
    void VisitAttributeNode(AttributeNode node);
}
