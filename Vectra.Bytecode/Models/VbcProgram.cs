namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a program or module in the Vectra Bytecode system.
/// </summary>
public class VbcProgram
{
    /// <summary>
    /// Gets the type of the module, represented as a <see cref="VbcModuleType"/> enumeration.
    /// </summary>
    /// <remarks>
    /// This property defines whether the module is an executable or a library in the Vectra Bytecode system.
    /// </remarks>
    public VbcModuleType ModuleType { get; init; }

    /// <summary>
    /// Gets the name of the module in the Vectra Bytecode system.
    /// </summary>
    /// <remarks>
    /// The <c>ModuleName</c> property represents the identifier or name assigned to a specific module.
    /// It is used to differentiate between various modules within the context of the program.
    /// This property is immutable and must be initialized when creating an instance of the <c>VbcProgram</c>.
    /// </remarks>
    public string ModuleName { get; init; }

    /// <summary>
    /// Represents the primary or root namespace of the program.
    /// </summary>
    /// <remarks>
    /// The RootSpace property defines the core namespace structure of the program,
    /// containing its types, sub-namespaces, and any other hierarchical elements
    /// relevant to the application architecture. It serves as the central container
    /// for all program definitions and resources within the Vectra Bytecode model.
    /// </remarks>
    public VbcSpace RootSpace { get; init; }

    /// <summary>
    /// Gets the name of the entry point method for the VbcProgram.
    /// This property specifies the main entry point method if the program is an executable
    /// (i.e., when <see cref="ModuleType"/> is set to <c>VbcModuleType.Executable</c>).
    /// If the module is a library (<c>VbcModuleType.Library</c>), this property may be null or unused.
    /// </summary>
    public string? EntryPointMethod { get; init; }
    
    public List<object> Constants { get; init; } = [];

    /// <summary>
    /// Gets the list of dependencies required by the program.
    /// </summary>
    /// <remarks>
    /// This property represents a collection of names for other modules or libraries
    /// that the current program depends upon. Each string in the list corresponds
    /// to a specific dependency, which could be a library or other resource.
    /// These dependencies are vital for ensuring the program's functionality and
    /// should be resolved during the build or execution process to avoid runtime errors.
    /// </remarks>
    /// <value>
    /// A list of strings representing the program's dependencies.
    /// </value>
    public List<string> Dependencies { get; init; }
}