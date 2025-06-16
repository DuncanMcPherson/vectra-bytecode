using System.Diagnostics.CodeAnalysis;
using Vectra.AST.Declarations;
using Vectra.AST.Statements;

namespace Vectra.Bytecode.Tests.TestUtils;

[ExcludeFromCodeCoverage]
public class MethodDeclarationNodeBuilder
{
    private string _name = "TestMethod";
    private List<Parameter> _parameters = [];
    private List<IStatementNode> _body = [];
    private string _returnType = "void";
    
    public MethodDeclarationNodeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public MethodDeclarationNodeBuilder WithParameters(List<Parameter> parameters)
    {
        _parameters = parameters;
        return this;
    }

    public MethodDeclarationNodeBuilder WithBody(List<IStatementNode> body)
    {
        _body = body;
        return this;
    }
    
    public MethodDeclarationNodeBuilder WithReturnType(string returnType)
    {
        _returnType = returnType;
        return this;       
    }
    
    public MethodDeclarationNode Build()
    {
        return new MethodDeclarationNode(_name, _parameters, _body, new(), _returnType);
    }
}