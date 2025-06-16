namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a method in the Vectra Bytecode model.
/// </summary>
/// <remarks>
/// A method in the Vectra Bytecode model includes its name, a collection of constants
/// associated with the method, parameters, and a sequence of instructions used to define the method's behavior.
/// </remarks>
public class VbcMethod
{
    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    /// <remarks>
    /// This property uniquely identifies the method within the context of the class or namespace.
    /// The value is immutable and is set during initialization.
    /// It is typically used for referencing or invoking the associated method.
    /// </remarks>
    public string Name { get; init; }

    /// <summary>
    /// Gets the collection of parameters associated with this method.
    /// </summary>
    /// <remarks>
    /// Each parameter in the collection is represented by an instance of the <see cref="VbcParameter"/> class,
    /// which includes both the name and the type of the parameter.
    /// </remarks>
    /// <value>
    /// A list of <see cref="VbcParameter"/> instances representing the method's parameters.
    /// </value>
    public List<VbcParameter> Parameters { get; init; } = [];
    
    public List<string> LocalVariables { get; init; } = [];

    /// <summary>
    /// Represents the set of low-level instructions associated with a method in the Vectra bytecode system.
    /// </summary>
    /// <remarks>
    /// Each instruction in the collection is defined using the <see cref="Instruction"/> struct.
    /// These instructions correspond to the operational logic of a method and include opcodes
    /// and their associated operands necessary for execution within the bytecode system.
    /// </remarks>
    /// <example>
    /// The <see cref="Instructions"/> property contains a sequential list that mirrors
    /// the execution process for the method it belongs to.
    /// </example>
    public List<Instruction> Instructions { get; init; } = [];
}