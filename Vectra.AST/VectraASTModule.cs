using Vectra.AST.Declarations;

namespace Vectra.AST;

/// <summary>
/// Represents a module node in the abstract syntax tree (AST) for Vectra.
/// </summary>
/// <remarks>
/// A <see cref="VectraASTModule"/> is a named, top-level construct in the AST, generally representing
/// a single module or unit of compilation. It has an associated root namespace or space declaration
/// that organizes the internal structure of the module. Modules can optionally be marked as executable,
/// allowing differentiation between executable and library modules.
/// </remarks>
public class VectraASTModule(string name, SpaceDeclarationNode space)
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

    /// <summary>
    /// Gets or sets a value indicating whether this module is executable.
    /// </summary>
    /// <remarks>
    /// The <see cref="IsExecutable"/> property determines if the module can be executed as part of
    /// the program's runtime behavior. This property is typically used to distinguish between modules
    /// that serve as executable entry points and those that are purely declarative or provide
    /// supporting functionality only.
    /// </remarks>
    public bool IsExecutable { get; set; } = true;

    /// <summary>
    /// Gets the root namespace or space declaration for this module.
    /// </summary>
    /// <remarks>
    /// The <see cref="RootSpace"/> property represents the top-level namespace or space declaration
    /// associated with the module. It organizes and structures the internal declarations, facilitating
    /// hierarchical groupings and access to nested namespaces or type definitions. This property provides
    /// an entry point into the module's logical structure within the abstract syntax tree (AST).
    /// </remarks>
    public SpaceDeclarationNode RootSpace { get; } = space;
}