namespace Vectra.Bytecode.Models;

/// <summary>
/// Specifies the type of module in the Vectra Bytecode system.
/// </summary>
public enum VbcModuleType : byte
{
    /// <summary>
    /// Represents a module type designed to be executed as a program
    /// </summary>
    Executable = 0x00,

    /// <summary>
    /// Represents a module type designed to be compiled as a library.
    /// </summary>
    Library = 0x01,
}