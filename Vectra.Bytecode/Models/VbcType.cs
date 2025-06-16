namespace Vectra.Bytecode.Models;

/// <summary>
/// Serves as an abstract base class for all Vectra Bytecode types.
/// </summary>
/// <remarks>
/// VbcType provides a foundational structure for defining the types
/// used within the Vectra Bytecode model. Classes inheriting from
/// VbcType can represent more specific constructs such as classes, methods,
/// or other components necessary within the bytecode framework. This class
/// enforces the presence of a Name property, ensuring each type has a consistent
/// identifying mechanism.
/// </remarks>
public abstract class VbcType
{
    /// <summary>
    /// Gets the name associated with the instance of the VbcType.
    /// </summary>
    /// <remarks>
    /// The Name property is immutable and must be set during object initialization.
    /// It represents the identifying or descriptive name for the specific VbcType instance.
    /// </remarks>
    public required string Name { get; init; }
}