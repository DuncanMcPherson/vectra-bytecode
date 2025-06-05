namespace Vectra.AST.Expressions;

/// <summary>
/// Represents a literal expression node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// The <see cref="LiteralExpressionNode"/> is used to encapsulate literal values within
/// the AST. This includes primitive values such as numbers, strings, or other types
/// of literals encountered in the source code. It derives from the <see cref="ExpressionNode"/>
/// class, indicating that it is a specific type of expression node within the AST hierarchy.
/// Each literal expression node contains a value representing the literal itself and
/// a <see cref="SourceSpan"/> property that indicates the precise location of the
/// literal in the source code. This enables accurate error reporting, debugging, and
/// code analysis.
/// </remarks>
public class LiteralExpressionNode(object value, SourceSpan span) : ExpressionNode(span)
{
    /// <summary>
    /// Gets the value represented by this literal expression node.
    /// </summary>
    /// <remarks>
    /// The <c>Value</c> property holds the underlying value of the literal expression.
    /// It represents the actual data stored in the literal, such as a number, string,
    /// boolean, or any object that corresponds to a literal in the source code.
    /// </remarks>
    /// <value>
    /// The value of the literal, as an <see cref="object"/>.
    /// </value>
    public object Value { get; } = value;

    /// Accepts a visitor to process the current LiteralExpressionNode.
    /// <param name="visitor">
    /// The visitor implementing IAstVisitor interface that will process this LiteralExpressionNode.
    /// </param>
    /// <typeparam name="T">
    /// The type of the result returned by the visitor.
    /// </typeparam>
    /// <returns>
    /// The result of processing this LiteralExpressionNode by the provided visitor.
    /// </returns>
    public override T Accept<T>(IAstVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}