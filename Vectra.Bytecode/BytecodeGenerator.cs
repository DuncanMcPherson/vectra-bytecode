using Vectra.AST;
using Vectra.Bytecode.Models;

namespace Vectra.Bytecode;

public class BytecodeGenerator : IAstVisitor<Unit>
{
    private List<Instruction> _instructions = [];
    private List<object> _constants = [];
    private Dictionary<string, int> _functionEntryPoints = [];
    private int _currentInstructionIndex = 0;

    public VbcProgram Generate(VectraASTModule module)
    {
        foreach (var space in module.Spaces)
            VisitSpace(space);

        return new VbcProgram
        {
            ModuleType = module.IsExecutable ? VbcModuleType.Executable : VbcModuleType.Library,
            ModuleName = module.Name,
            Instructions = _instructions,
            Constants = _constants,
            EntryPointMethod = module.IsExecutable ? "Program.main" : null,
            Exports = _functionEntryPoints.Keys.ToList(),
            Dependencies = new List<string>() // TODO: Populate this list with dependencies
        };
    }

    private void Emit(OpCode opcode, int operand = 0)
    {
        _instructions.Add(new Instruction(opcode, operand));
        _currentInstructionIndex++;
    }
}