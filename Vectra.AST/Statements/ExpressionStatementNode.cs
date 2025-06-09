using Vectra.AST.Expressions;
using Vectra.AST.Models;

namespace Vectra.AST.Statements;

/// <summary>
/// Represents a statement node in the abstract syntax tree (AST) where an expression is executed as a statement.
/// </summary>
/// <remarks>
/// The <see cref="ExpressionStatementNode"/> class is used to wrap an expression inside a statement context.
/// This allows expressions, such as method calls or assignments, to be treated as standalone statements within
/// the program's structure. It inherits from the <see cref="StatementNode"/> base class, providing positional
/// information via the <see cref="SourceSpan"/> property.
/// An instance of <see cref="ExpressionStatementNode"/> contains a reference to an <see cref="ExpressionNode"/> object,
/// which represents the encapsulated expression. The associated source code region for the statement is defined by the
/// <see cref="SourceSpan"/> inherited from <see cref="StatementNode"/>.
/// This class is primarily utilized in abstract syntax trees (ASTs) generated during the compilation or interpretation
/// process, enabling both execution and further transformations of expression-based statements.
/// </remarks>
/// <example>
/// This class may represent expressions like:
/// 1. A function call: `foo();`
/// 2. An assignment: `x = 5;`
/// 3. An increment operation: `i++;`
/// </example>
public class ExpressionStatementNode(ExpressionNode expression, SourceSpan span) : StatementNode(span)
{
    /// <summary>
    /// Represents the expression component within an expression statement node in the abstract syntax tree (AST).
    /// </summary>
    /// <remarks>
    /// The <c>Expression</c> property encapsulates an instance of <see cref="ExpressionNode"/> tied to the
    /// parent <see cref="ExpressionStatementNode"/>. It defines the specific expression being evaluated or executed
    /// within the context of a statement. This property is immutable and initialized through the constructor
    /// of the <c>ExpressionStatementNode</c>.
    /// </remarks>
    public ExpressionNode Expression { get; } = expression;

    /// <summary>
    /// Accepts a visitor implementing the <see cref="IAstVisitor{T}"/> interface and allows dispatching
    /// of the visitor's logic specific to this <see cref="ExpressionStatementNode"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of value returned by the visitor.</typeparam>
    /// <param name="visitor">The visitor implementing the <see cref="IAstVisitor{T}"/> interface to be accepted.</param>
    /// <returns>The result of the visitor's operation on this <see cref="ExpressionStatementNode"/> instance.</returns>
    public override T Accept<T>(IAstVisitor<T> visitor)
    {
        return visitor.VisitExpressionStatement(this);
    }
}