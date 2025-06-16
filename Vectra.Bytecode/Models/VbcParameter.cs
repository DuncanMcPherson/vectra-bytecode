namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a parameter in the Vectra Bytecode model.
/// </summary>
/// <remarks>
/// A parameter in the Vectra Bytecode model consists of a name and its associated type.
/// </remarks>
public class VbcParameter
{
    /// <summary>
    /// Represents the name of the parameter in the VbcParameter class.
    /// </summary>
    /// <remarks>
    /// This property is used to store the name of a parameter. It is immutable after
    /// initialization, ensuring that the value remains consistent throughout the lifetime
    /// of the VbcParameter instance.
    /// </remarks>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the name of the type associated with the parameter.
    /// </summary>
    /// <remarks>
    /// This property represents the type information for a given parameter
    /// within the VbcParameter class. It is used to identify the type of
    /// the parameter as part of the bytecode model.
    /// </remarks>
    public required string TypeName { get; init; }
}