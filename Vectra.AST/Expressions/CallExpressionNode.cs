using Vectra.AST.Statements;

namespace Vectra.AST.Expressions;

/// <summary>
/// Represents a call expression node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// A CallExpressionNode encapsulates an expression representing the target of the call,
/// a list of expressions representing the arguments to the call, and the source code span
/// associated with the node. This class is used to model function or method invocation
/// expressions in the AST.
/// </remarks>
public class CallExpressionNode(ExpressionNode target, List<ExpressionNode> arguments, SourceSpan span) : StatementNode(span)
{
    /// <summary>
    /// Gets the target expression for the call expression.
    /// </summary>
    /// <remarks>
    /// The <c>Target</c> property represents the expression that is
    /// being invoked in the call. This is the primary callable object
    /// or function within the call expression, such as a method or
    /// function reference.
    /// </remarks>
    public ExpressionNode Target { get; } = target;

    /// Gets the list of arguments associated with a call expression.
    /// This property contains the collection of `ExpressionNode` objects representing
    /// the arguments passed to a function or method call within the abstract syntax tree (AST).
    /// The arguments are typically evaluated in the order they appear in the list.
    /// Each `ExpressionNode` in the list is an independent expression that corresponds
    /// to an individual argument in the call.
    /// This property is read-only.
    public List<ExpressionNode> Arguments { get; } = arguments;

    /// Accepts a visitor implementing the IAstVisitor interface and invokes the
    /// corresponding visit method for the CallExpressionNode.
    /// <param name="visitor">The visitor that implements the IAstVisitor interface.</param>
    /// <typeparam name="T">The type returned by the visitor's VisitCallExpression method.</typeparam>
    /// <returns>Returns an object of type T as specified by the visitor.</returns>
    public override T Accept<T>(IAstVisitor<T> visitor)
    {
        return visitor.VisitCallExpression(this);
    }
}