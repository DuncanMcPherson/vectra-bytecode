namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a low-level instruction used in the bytecode execution system.
/// </summary>
public readonly struct Instruction
{
    /// <summary>
    /// Represents the operational code (OpCode) for an instruction in the bytecode model.
    /// </summary>
    /// <remarks>
    /// The <see cref="OpCode"/> property specifies the operation to be performed by the instruction.
    /// It is an enumerated value defined in the <see cref="Vectra.Bytecode.Models.OpCode"/> enum.
    /// </remarks>
    /// <value>
    /// A value of type <see cref="OpCode"/> that determines the type of operation,
    /// such as arithmetic operations, function calls, or data manipulation within the bytecode system.
    /// </value>
    public OpCode OpCode { get; }

    /// <summary>
    /// Gets the operand associated with the instruction.
    /// </summary>
    /// <remarks>
    /// The operand represents an integer value that provides additional
    /// information or context required by the <see cref="OpCode"/>. This value
    /// can vary depending on the specific operation being performed.
    /// </remarks>
    /// <value>
    /// An integer representing the operand associated with the instruction. The
    /// exact interpretation of this value depends on the <see cref="OpCode"/>.
    /// </value>
    public int Operand { get; }

    /// <summary>
    /// Represents a single bytecode instruction used in the Vectra bytecode system.
    /// </summary>
    public Instruction(OpCode opCode, int operand = 0)
    {
        OpCode = opCode;
        Operand = operand;
    }
}