using System.Diagnostics.CodeAnalysis;
using Vectra.AST.Models;

namespace Vectra.AST.Statements;

/// <summary>
/// Represents the base class for all statement nodes in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// This class encapsulates the source span information for the statement
/// and is inherited by all statement-specific node types.
/// StatementNode is abstract and cannot be instantiated directly.
/// </remarks>
public abstract class StatementNode : IAstNode
{
    /// <summary>
    /// Gets the span of source code associated with the AST node.
    /// </summary>
    /// <remarks>
    /// The <c>Span</c> property represents the source location information
    /// for the node within the abstract syntax tree (AST). It provides
    /// details like the start and end line and column positions in the source text.
    /// This property is implemented from the <c>IAstNode</c> interface.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public SourceSpan Span { get; }

    /// <summary>
    /// Represents the base class for all statement nodes in the Abstract Syntax Tree (AST).
    /// </summary>
    /// <remarks>
    /// StatementNode serves as an abstract base class, providing a common structure for different types of statements
    /// within the AST. It defines a source span that describes the location of the statement in the source code.
    /// </remarks>
    protected StatementNode(SourceSpan span) => Span = span;

    /// <summary>
    /// Accepts a visitor that implements the <see cref="IAstVisitor{T}"/> interface and allows the visitor to process the current node.
    /// </summary>
    /// <typeparam name="T">The type of value produced by the visitor.</typeparam>
    /// <param name="visitor">The visitor instance responsible for processing the node.</param>
    /// <returns>A value of type <typeparamref name="T"/> produced by the visitor.</returns>
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}