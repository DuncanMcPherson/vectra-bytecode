namespace Vectra.Bytecode.Models;

public class VbcField
{
    public required string Name { get; init; }
    // TODO: Consider handling type resolution and replacing
    // the `FieldDeclarationNode.Type` with a defined type during type analysis
    public required string? TypeName { get; init; } // This will be null if we are inferring via `let`
    public required object? InitialValue { get; init; } // This will be null if there is no initializer (required if `let`)
}