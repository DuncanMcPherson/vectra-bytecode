using Vectra.AST.Declarations.Interfaces;
using Vectra.AST.Models;

namespace Vectra.AST.Declarations;

/// <summary>
/// Represents a class declaration node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// This node encapsulates the name, member definitions, and source location
/// of a class within the code structure. It implements the <see cref="ITypeDeclarationNode"/>
/// interface, which allows it to be identified as a type declaration.
/// </remarks>
public class ClassDeclarationNode(string name, List<IMemberNode> members, SourceSpan span) : ITypeDeclarationNode
{
    /// Gets the name of the node or declaration.
    /// This property represents the identifier name for the class, space, or member node.
    /// It provides a human-readable name to distinguish specific elements in the abstract syntax tree.
    public string Name { get; } = name;

    /// <summary>
    /// Represents the collection of member nodes declared within a class.
    /// </summary>
    /// <remarks>
    /// This property provides access to all the members defined in the class represented by the
    /// <see cref="ClassDeclarationNode"/>. Each member is an implementation of the <see cref="IMemberNode"/>
    /// interface, which ensures that the collection maintains a consistent structure of node types.
    /// </remarks>
    /// <value>
    /// A list of <see cref="IMemberNode"/> objects representing the members of the class.
    /// These members can include fields, methods, properties, or other types, depending on the implementation.
    /// </value>
    public List<IMemberNode> Members { get; } = members;

    /// Represents the span of source code related to this node.
    /// The Span property provides information about the start and end
    /// positions of a syntax element in the source code. It is useful
    /// for error reporting, diagnostics, and other analyses that require
    /// precise location details. The SourceSpan structure includes line
    /// and column information for both the start and end positions.
    /// This property is particularly significant for tools that manipulate
    /// or analyze abstract syntax trees (ASTs), as it allows tracing
    /// specific parts of the source code accurately. It is accessible
    /// through various node types in the AST.
    public SourceSpan Span { get; } = span;

    /// <summary>
    /// Accepts a visitor that implements the IAstVisitor interface and allows
    /// the visitor to process the current node.
    /// </summary>
    /// <typeparam name="T">The return type of the visitor's method.</typeparam>
    /// <param name="visitor">An instance of a class implementing the IAstVisitor interface.</param>
    /// <returns>Returns the result of the visitor's operation as type <typeparamref name="T"/>.</returns>
    public T Accept<T>(IAstVisitor<T> visitor) => visitor.VisitClassDeclaration(this);
}