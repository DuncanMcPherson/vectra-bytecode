namespace Vectra.AST;

/// <summary>
/// Represents a node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// The <c>IAstNode</c> interface serves as a base for all nodes in the AST. It defines a
/// common structure for associating nodes with their respective source code spans.
/// </remarks>
public interface IAstNode
{
    /// <summary>
    /// Gets the range of the source code associated with the node.
    /// </summary>
    /// <remarks>
    /// The <c>Span</c> property represents the source position of the node within the file,
    /// encompassing the start and end line, as well as the start and end column.
    /// This allows precise tracking of node locations in the source code, which can be
    /// useful for error reporting, debugging, and code analysis.
    /// </remarks>
    SourceSpan Span { get; }

    /// <summary>
    /// Accepts a visitor that implements the <c>IAstVisitor</c> interface and allows traversal
    /// or processing of the current abstract syntax tree (AST) node.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the visitor upon visiting the node.</typeparam>
    /// <param name="visitor">
    /// The visitor that implements the desired logic for processing the AST node.
    /// It must implement the <c>IAstVisitor&lt;T&gt;</c> interface.
    /// </param>
    /// <returns>
    /// The result produced by the visitor after processing the current node.
    /// The type of the result is determined by the visitor's implementation.
    /// </returns>
    T Accept<T>(IAstVisitor<T> visitor);
}