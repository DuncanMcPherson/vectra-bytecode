using System.Diagnostics.CodeAnalysis;
using Vectra.AST;
using Vectra.AST.Declarations;
using Vectra.AST.Declarations.Interfaces;
using Vectra.AST.Expressions;
using Vectra.AST.Statements;
using Vectra.Bytecode.Models;

namespace Vectra.Bytecode;

public class BytecodeGenerator : IAstVisitor<Unit>
{
    private List<object> _currentConstants = [];
    private List<Instruction> _currentInstructions = [];
    private List<string> _localVariables = [];
    public VbcProgram Generate(VectraASTModule module)
    {
        return new VbcProgram
        {
            Dependencies = [],
            EntryPointMethod = module.IsExecutable ? "Program.main" : null,
            ModuleName = module.Name,
            ModuleType = module.IsExecutable ? VbcModuleType.Executable : VbcModuleType.Library,
            RootSpace = WalkSpace(module.RootSpace),
        };
    }

    private VbcSpace WalkSpace(SpaceDeclarationNode space)
    {
        ArgumentNullException.ThrowIfNull(space);
        List<VbcType> types = [];
        List<VbcSpace> subspaces = [];
        types.AddRange(space.Declarations.Select(WalkDeclaration));

        subspaces.AddRange(space.ChildSpaces.Select(WalkSpace));

        return new VbcSpace
        {
            Name = space.QualifiedName,
            Types = types,
            Subspaces = subspaces
        };
    }

    private VbcType WalkDeclaration(ITypeDeclarationNode type)
    {
        return type switch
        {
            ClassDeclarationNode cls => WalkClass(cls),
            _ => throw new ArgumentException($"Invalid or unsupported type declaration: {type.GetType().Name}")
        };
    }

    private VbcClass WalkClass(ClassDeclarationNode cls)
    {
        return new VbcClass
        {
            Name = cls.Name,
            Methods = cls.Members
                .OfType<MethodDeclarationNode>()
                .Select(WalkMethod)
                .ToList()
        };
    }

    [ExcludeFromCodeCoverage]
    public Unit VisitClassDeclaration(ClassDeclarationNode node)
    {
        WalkClass(node);
        return Unit.Value;
    }

    [ExcludeFromCodeCoverage]
    public Unit VisitMethodDeclaration(MethodDeclarationNode node)
    {
        WalkMethod(node);
        return Unit.Value;
    }

    public Unit VisitExpressionStatement(ExpressionStatementNode node)
    {
        node.Expression.Accept(this);
        _currentInstructions.Add(new Instruction(OpCode.Pop));
        return Unit.Value;
    }

    public Unit VisitReturnStatement(ReturnStatementNode node)
    {
        node.Value?.Accept(this);
        
        _currentInstructions.Add(new Instruction(OpCode.Ret));
        return Unit.Value;
    }

    public Unit VisitLiteralExpression(LiteralExpressionNode node)
    {
        var index = _currentConstants.Count;
        _currentConstants.Add(node.Value);
        _currentInstructions.Add(new Instruction(OpCode.LoadConst, index));
        return Unit.Value;
    }

    public Unit VisitIdentifierExpression(IdentifierExpressionNode node)
    {
        var index = _localVariables.IndexOf(node.Name);
        if (index == -1)
        {
            index = _localVariables.Count;
            _localVariables.Add(node.Name);
        }
        _currentInstructions.Add(new Instruction(OpCode.LoadLocal, index));
        return Unit.Value;
    }

    public Unit VisitCallExpression(CallExpressionNode node)
    {
        node.Target.Accept(this);
        foreach (var nodeArgument in node.Arguments)
        {
            nodeArgument.Accept(this);
        }
        
        var methodNameIndex = AddConstant(node.MethodName);
        
        _currentInstructions.Add(new Instruction(OpCode.Call, methodNameIndex));

        return Unit.Value;
    }

    public Unit VisitBinaryExpression(BinaryExpressionNode node)
    {
        node.Left.Accept(this);
        node.Right.Accept(this);

        var op = node.Operator switch
        {
            "+" => OpCode.Add,
            "-" => OpCode.Sub,
            "*" => OpCode.Mul,
            "/" => OpCode.Div,
            "==" => OpCode.Eq,
            "!=" => OpCode.Neq,
            "<" => OpCode.Lt,
            "<=" => OpCode.Leq,
            ">" => OpCode.Gt,
            ">=" => OpCode.Geq,
            _ => throw new NotSupportedException($"Binary operator '{node.Operator}' is not supported.")
        };
        _currentInstructions.Add(new Instruction(op));
        return Unit.Value;
    }

    private VbcMethod WalkMethod(MethodDeclarationNode method)
    {
        var parameters = method.Parameters.Select(type => new VbcParameter { Name = type.Name, TypeName = type.Type }).ToList(); // TODO: Update method.Parameters to provide both param name and type
        _currentConstants = [];
        _currentInstructions = [];
        _localVariables =
        [
            "this"
        ];

        foreach (var parameter in parameters)
        {
            _localVariables.Add(parameter.Name);
        }

        foreach (var statement in method.Body)
        {
            statement.Accept(this);
        }

        return new VbcMethod
        {
            Name = method.Name,
            Constants = _currentConstants,
            Instructions = _currentInstructions,
            LocalVariables = _localVariables,
            Parameters = parameters
        };
    }
    
    private int AddConstant(object value)
    {
        var index = _currentConstants.IndexOf(value);
        if (index != -1) return index;
        index = _currentConstants.Count;
        _currentConstants.Add(value);
        return index;
    }
}