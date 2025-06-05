using Vectra.AST.Declarations;

namespace Vectra.AST;

/// <summary>
/// Represents a module in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// The <c>VectraASTModule</c> class is used to encapsulate the name and associated
/// namespaces or spaces within a module. It aggregates a collection of
/// <see cref="SpaceDeclarationNode"/> objects, which define the logical
/// structure and declarations within the module.
/// </remarks>
public class VectraASTModule(string name, List<SpaceDeclarationNode> spaces)
{
    /// <summary>
    /// Gets the name of this module.
    /// </summary>
    /// <remarks>
    /// The <see cref="Name"/> property represents the identifier for this module in the abstract
    /// syntax tree (AST). It is typically used to uniquely distinguish the module within the AST
    /// structure and can act as a reference for organizing or analyzing declarations.
    /// </remarks>
    public string Name { get; } = name;
    
    public bool IsExecutable { get; set; } = true;

    /// <summary>
    /// Gets the collection of space declarations associated with the module.
    /// </summary>
    /// <remarks>
    /// The <c>Spaces</c> property contains a list of <see cref="SpaceDeclarationNode"/> objects that define the
    /// namespaces or logical groupings in this module. These declarations represent the structure and hierarchy of
    /// the abstract syntax tree (AST) within the module. Each space declaration may include nested spaces and type declarations.
    /// </remarks>
    public List<SpaceDeclarationNode> Spaces { get; } = spaces;
}