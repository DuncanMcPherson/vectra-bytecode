namespace Vectra.AST.Declarations;

/// <summary>
/// Represents a type declaration node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// This interface is implemented by nodes that define types within the AST,
/// such as class declarations. It provides the minimal contract for naming
/// and basic structural information about a type declaration.
/// </remarks>
public interface ITypeDeclarationNode : IAstNode
{
    /// <summary>
    /// Gets the name associated with the current node.
    /// </summary>
    /// <remarks>
    /// The <c>Name</c> property represents the identifier or name of the entity
    /// defined by the implementing node. For example, in a <c>ClassDeclarationNode</c>,
    /// it represents the class's name, whereas in a <c>InterfaceDeclarationNode</c> it represents
    /// the interface's name.
    /// </remarks>
    string Name { get; }
}