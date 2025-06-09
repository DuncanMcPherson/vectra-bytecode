using Vectra.AST.Expressions;
using Vectra.AST.Models;

namespace Vectra.AST.Statements;

/// <summary>
/// Represents a return statement in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// The <c>ReturnStatementNode</c> is a type of statement node that corresponds to a return statement in the source code.
/// It optionally contains an associated expression, represented by the <see cref="Value"/> property, indicating the value
/// to be returned by the return statement. If no value is provided, the return statement is treated as returning no value.
/// </remarks>
public class ReturnStatementNode(ExpressionNode? value, SourceSpan span) : StatementNode(span)
{
    /// <summary>
    /// Gets the expression node representing the value to be returned.
    /// </summary>
    /// <remarks>
    /// The <c>Value</c> property represents the optional expression that corresponds to the value
    /// being returned in a return statement. It is an instance of the <see cref="ExpressionNode"/> class
    /// or null if no value is specified in the return statement.
    /// If the return statement includes an expression (e.g., <c>return 42;</c>), the <c>Value</c>
    /// property will contain the corresponding expression node. In the absence of a specified value
    /// (e.g., <c>return;</c>), this property will be null.
    /// Use this property to analyze or transform the return statement's value in the abstract syntax tree (AST).
    /// </remarks>
    public ExpressionNode? Value { get; } = value;

    /// Represents a method that accepts a visitor and allows it to process the current node in the AST.
    /// <typeparam name="T">The return type of the visitor's operation.</typeparam>
    /// <param name="visitor">The visitor that will process this node.</param>
    /// <returns>Returns an instance of type T, which is the result of processing by the visitor.</returns>
    public override T Accept<T>(IAstVisitor<T> visitor)
    {
        return visitor.VisitReturnStatement(this);
    }
}