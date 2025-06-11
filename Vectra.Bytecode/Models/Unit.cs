namespace Vectra.Bytecode.Models;

/// <summary>
/// Represents a singleton unit value, commonly used to signify a single return type
/// in situations where no other value is required.
/// </summary>
/// <remarks>
/// The <see cref="Unit"/> struct is immutable and contains only a single static instance, <see cref="Value"/>.
/// It is useful in scenarios such as signaling completion or acting as a placeholder without carrying additional data.
/// </remarks>
/// <threadsafety>
/// This type is thread-safe due to its immutability and the use of a static, readonly field for the instance.
/// </threadsafety>
public readonly struct Unit
{
    /// <summary>
    /// Represents the default and only instance of the <see cref="Unit"/> struct.
    /// This field provides a value to be used when no specific context or data is required.
    /// </summary>
    public static readonly Unit Value = new();
}