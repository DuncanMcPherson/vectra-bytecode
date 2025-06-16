using System.Diagnostics.CodeAnalysis;
using Vectra.AST;
using Vectra.AST.Declarations;

namespace Vectra.Bytecode.Tests.TestUtils;

[ExcludeFromCodeCoverage]
public class VectraASTModuleBuilder
{
    private string _name = "Test";
    private SpaceDeclarationNode _space = null!;
    private bool _isExecutable;
    
    public VectraASTModuleBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public VectraASTModuleBuilder WithSpace(SpaceDeclarationNode space)
    {
        _space = space;
        return this;
    }
    
    public VectraASTModuleBuilder WithIsExecutable(bool isExecutable)
    {
        _isExecutable = isExecutable;
        return this;
    }
    
    public VectraASTModule Build()
    {
        return new VectraASTModule(_name, _space)
        {
            IsExecutable = _isExecutable
        };
    }
}