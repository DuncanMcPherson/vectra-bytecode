namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a namespace-like structure in the Vectra Bytecode system.
/// </summary>
/// <remarks>
/// A VbcSpace serves as a container for related types and subspaces, allowing for hierarchical
/// organization within the bytecode model.
/// </remarks>
public class VbcSpace
{
    /// <summary>
    /// Gets the name of the VbcSpace, which serves as its identifier or description.
    /// This property is immutable and must be initialized during the creation of the instance.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Represents a collection of types associated with a specific space.
    /// Each type contained within the collection is defined to extend
    /// the abstract base class <see cref="VbcType"/>. This property is
    /// useful for categorizing and managing multiple VbcType instances
    /// within the context of a given VbcSpace.
    /// </summary>
    /// <remarks>
    /// Instances of <see cref="VbcType"/> in this collection represent
    /// specific behaviors or properties relevant to the containing
    /// VbcSpace. The property is immutable after initialization, ensuring
    /// stability of the types associated with the space after creation.
    /// </remarks>
    /// <value>
    /// A list of objects of type <see cref="VbcType"/>. Returns an empty
    /// list if no types are defined for the space.
    /// </value>
    public List<VbcType> Types { get; init; }

    /// <summary>
    /// Gets the collection of subspaces belonging to the current <see cref="VbcSpace"/> instance.
    /// </summary>
    /// <remarks>
    /// Each subspace is represented as a <see cref="VbcSpace"/> object, allowing hierarchical organization of bytecode spaces.
    /// This property can be used to traverse and manage a tree-like structure of nested bytecode spaces.
    /// </remarks>
    public List<VbcSpace> Subspaces { get; init; }
}