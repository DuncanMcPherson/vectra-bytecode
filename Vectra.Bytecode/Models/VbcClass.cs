namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a class in the Vectra Bytecode model, extending the base functionality provided by <see cref="VbcType"/>.
/// </summary>
/// <remarks>
/// VbcClass defines a structure that contains methods and inherits core properties from the VbcType class.
/// </remarks>
public class VbcClass : VbcType
{
    /// <summary>
    /// Gets the collection of methods associated with the class.
    /// Each method is represented by an instance of <see cref="VbcMethod"/>,
    /// which includes details such as the method name and its associated constants.
    /// </summary>
    /// <remarks>
    /// This property is initialized during the creation of the class object and is immutable thereafter.
    /// The list provides access to all methods defined within the class, offering
    /// a structural representation for analyzing or processing the methods.
    /// </remarks>
    public required List<VbcMethod> Methods { get; init; }
}