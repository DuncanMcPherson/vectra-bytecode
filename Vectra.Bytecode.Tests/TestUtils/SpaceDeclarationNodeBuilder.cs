using System.Diagnostics.CodeAnalysis;
using Vectra.AST.Declarations;
using Vectra.AST.Declarations.Interfaces;

namespace Vectra.Bytecode.Tests.TestUtils;

[ExcludeFromCodeCoverage]
public class SpaceDeclarationNodeBuilder
{
    private string _name = "Test";
    private List<ITypeDeclarationNode> _declarations = [];
    private SpaceDeclarationNode? _parent;
    
    public SpaceDeclarationNodeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public SpaceDeclarationNodeBuilder WithDeclarations(List<ITypeDeclarationNode> declarations)
    {
        _declarations = declarations;
        return this;
    }
    
    public SpaceDeclarationNodeBuilder WithParent(SpaceDeclarationNode parent)
    {
        _parent = parent;
        return this;
    }
    
    public SpaceDeclarationNode Build()
    {
        return new SpaceDeclarationNode(_name, _declarations, new(), _parent);
    }
}