using Vectra.AST.Models;

namespace Vectra.AST.Expressions;

/// <summary>
/// Represents a base class for all expression nodes in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// The ExpressionNode class provides a common base for specific types of expressions in the AST.
/// It implements the <see cref="IAstNode"/> interface and includes a <see cref="SourceSpan"/>
/// property to define the span of source code associated with the expression.
/// </remarks>
public abstract class ExpressionNode : IAstNode
{
    /// <summary>
    /// Gets the source span associated with this node.
    /// </summary>
    /// <remarks>
    /// The <see cref="Span"/> property represents the range of source code that this node
    /// occupies within the abstract syntax tree (AST). It can be utilized to determine
    /// the exact location (start line, start column, end line, end column) of the node
    /// in the source file. This is particularly useful for debugging, diagnostics, or
    /// providing detailed error messages to users.
    /// The property is immutable and defined during the construction of the node.
    /// </remarks>
    public SourceSpan Span { get; }

    /// Represents an abstract base class for all expression nodes in the abstract syntax tree (AST).
    /// Provides a common interface and properties shared by all concrete expression nodes.
    /// An expression node is associated with a specific region in the source code, defined using the
    /// `SourceSpan` property. The `Span` property encapsulates the start and end positions for the
    /// node in terms of line and column numbers, allowing precise error reporting or code analysis.
    /// This class is intended to be used as a base class for other specific expression types.
    /// It inherits from `IAstNode`, making it part of the AST structure.
    /// Derived classes should pass a valid `SourceSpan` object to this base class's constructor
    /// to initialize the `Span` property.
    protected ExpressionNode(SourceSpan span) => Span = span;
    
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}