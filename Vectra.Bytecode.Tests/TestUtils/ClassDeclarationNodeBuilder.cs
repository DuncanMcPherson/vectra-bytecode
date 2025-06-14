using System.Diagnostics.CodeAnalysis;
using Vectra.AST.Declarations;
using Vectra.AST.Declarations.Interfaces;

namespace Vectra.Bytecode.Tests.TestUtils;

[ExcludeFromCodeCoverage]
public class ClassDeclarationNodeBuilder
{
    private string _name = "TestClass";
    private List<IMemberNode> _members = [];
    
    public ClassDeclarationNodeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public ClassDeclarationNodeBuilder WithMembers(List<IMemberNode> members)
    {
        _members = members;
        return this;
    }
    
    public ClassDeclarationNode Build()
    {
        return new ClassDeclarationNode(_name, _members, new());
    }
}