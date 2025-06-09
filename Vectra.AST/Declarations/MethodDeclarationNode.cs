using Vectra.AST.Statements;
using Vectra.AST.Declarations.Interfaces;

namespace Vectra.AST.Declarations;

/// <summary>
/// Represents the declaration of a method in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// This class defines a method declaration, including its name, parameters,
/// return type, body, and its span in the source code. It is used to model
/// the structure and metadata of method declarations in the AST.
/// </remarks>
public class MethodDeclarationNode(
    string name,
    List<string> parameters,
    List<StatementNode> body,
    SourceSpan span,
    string returnType)
    : IMemberNode
{
    /// <summary>
    /// Gets the name of the entity represented by this node.
    /// </summary>
    /// <remarks>
    /// The <c>Name</c> property provides the identifier for the entity, such as a method, class, or
    /// member, within the abstract syntax tree (AST). It ensures that this entity can be referenced
    /// and differentiated from other nodes within the same scope or structure.
    /// </remarks>
    /// <value>
    /// A read-only string representing the name of the entity.
    /// </value>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the list of parameter names for the method.
    /// </summary>
    /// <remarks>
    /// This property contains the names of the parameters that the method declaration accepts.
    /// The parameters are represented as a list of strings, where each string corresponds
    /// to the name of a parameter in the method's signature.
    /// </remarks>
    public List<string> Parameters { get; } = parameters;

    /// <summary>
    /// Gets the return type of the method declared by this node.
    /// </summary>
    /// <remarks>
    /// This property represents the data type of the value returned by the method.
    /// It is defined as a string to allow flexibility for a range of type representations,
    /// including custom or user-defined types.
    /// The return type information is critical for static analysis, code generation,
    /// or validation in the abstract syntax tree (AST) processing contexts.
    /// </remarks>
    public string ReturnType { get; } = returnType;

    /// <summary>
    /// Gets the list of statements that represent the body of the method.
    /// </summary>
    /// <remarks>
    /// The Body property contains a collection of <see cref="StatementNode"/> objects that define
    /// the functional logic of the method. Each statement within the body represents a discrete
    /// action or logical operation executed in the method's implementation.
    /// </remarks>
    /// <value>
    /// A list of <see cref="StatementNode"/> instances that constitutes the method's body.
    /// </value>
    public List<StatementNode> Body { get; } = body;

    /// <summary>
    /// Gets the source span of the node, which represents the location of the element
    /// in the source code.
    /// </summary>
    /// <remarks>
    /// The <see cref="SourceSpan"/> property provides detailed information about the
    /// starting and ending locations of the node within the source code. This
    /// includes the starting line and column, as well as the ending line and column,
    /// allowing precise identification of its position.
    /// </remarks>
    public SourceSpan Span { get; } = span;

    /// Accepts a visitor that implements the IAstVisitor interface and processes the current MethodDeclarationNode instance.
    /// <typeparam name="T">
    /// The type of the result returned by the visitor.
    /// </typeparam>
    /// <param name="visitor">
    /// The visitor that will visit the current MethodDeclarationNode instance.
    /// </param>
    /// <returns>
    /// Returns a result of type T, as produced by the visitor when processing this MethodDeclarationNode.
    /// </returns>
    public T Accept<T>(IAstVisitor<T> visitor) => visitor.VisitMethodDeclaration(this);
}