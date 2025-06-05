using Vectra.AST.Declarations;
using Vectra.AST.Expressions;
using Vectra.AST.Statements;

namespace Vectra.AST;

/// <summary>
/// Defines a visitor interface for traversing and processing various nodes
/// in the Abstract Syntax Tree (AST). The interface follows the Visitor design
/// pattern and allows implementing classes to define specific behaviors for
/// each type of AST node by overriding the corresponding visit methods.
/// </summary>
/// <typeparam name="T">
/// The type of the result returned by each visit method. For example,
/// this could be a processed representation of the node or a value derived
/// from the traversal.
/// </typeparam>
public interface IAstVisitor<out T>
{
    /// <summary>
    /// Visits a class declaration node in the abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">
    /// The <see cref="ClassDeclarationNode"/> representing the class declaration to be visited.
    /// </param>
    /// <returns>
    /// A generic result of the visitor's operation, determined by the implementation.
    /// </returns>
    T VisitClassDeclaration(ClassDeclarationNode node);

    /// <summary>
    /// Visits a method declaration node in the abstract syntax tree (AST) and performs
    /// an operation defined by the implementation.
    /// </summary>
    /// <param name="node">The method declaration node to visit. This contains information
    /// such as the method name, parameters, return type, body, and source span.</param>
    /// <returns>An object of type <typeparamref name="T"/> representing the result of the visitation
    /// process, as defined by the implementation.</returns>
    T VisitMethodDeclaration(MethodDeclarationNode node);

    /// Visits an expression statement node during traversal of the abstract syntax tree (AST).
    /// <param name="node">
    /// The expression statement node to visit. This node encapsulates an expression as its main component
    /// and may also include source span information indicating its location in the source code.
    /// </param>
    /// <returns>
    /// A value of the type specified by the generic parameter of the visitor interface. This is typically the result of processing or transforming the node.
    /// </returns>
    T VisitExpressionStatement(ExpressionStatementNode node);

    /// <summary>
    /// Visits a return statement node in the abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">
    /// The <see cref="ReturnStatementNode"/> representing the return statement to be visited.
    /// This node may include an optional expression indicating the value to be returned.
    /// </param>
    /// <returns>
    /// A result of the visit operation determined by the implementation.
    /// The return type is defined by the type parameter of the <see cref="IAstVisitor{T}"/> interface.
    /// </returns>
    T VisitReturnStatement(ReturnStatementNode node);

    /// <summary>
    /// Visits a literal expression node in the abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">
    /// The <see cref="LiteralExpressionNode"/> representing the literal expression being visited.
    /// This node encapsulates a literal value and its corresponding location in the source code.
    /// </param>
    /// <returns>
    /// Returns a value of type <see cref="T"/> that represents the result of visiting
    /// the literal expression node. The returned value depends on the specific implementation of the visitor.
    /// </returns>
    T VisitLiteralExpression(LiteralExpressionNode node);

    /// <summary>
    /// Visits an identifier expression node within an abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">The <see cref="IdentifierExpressionNode"/> to visit. This node represents an identifier within the AST.</param>
    /// <returns>
    /// A result of type <typeparamref name="T"/> based on the processing of the identifier expression.
    /// </returns>
    T VisitIdentifierExpression(IdentifierExpressionNode node);

    /// <summary>
    /// Visits a call expression node in the abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">The <see cref="CallExpressionNode"/> to visit. Represents a function or method call,
    /// including its target expression and arguments.</param>
    /// <returns>
    /// Returns a generic type <typeparamref name="T"/> to represent the result of visiting the
    /// <see cref="CallExpressionNode"/>. This is typically determined by the implementation of the visitor.
    /// </returns>
    T VisitCallExpression(CallExpressionNode node);

    /// <summary>
    /// Visits a binary expression node in the abstract syntax tree (AST).
    /// </summary>
    /// <param name="node">The binary expression node to be visited, containing the operator, left-hand operand, and right-hand operand.</param>
    /// <returns>Returns a result of type <typeparamref name="T"/> based on the implementation of the visitor.</returns>
    T VisitBinaryExpression(BinaryExpressionNode node);
}