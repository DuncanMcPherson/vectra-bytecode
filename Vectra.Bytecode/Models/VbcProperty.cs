namespace Vectra.Bytecode.Models;

public class VbcProperty
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required bool HasGetter { get; init; }
    public required bool HasSetter { get; init; }
}