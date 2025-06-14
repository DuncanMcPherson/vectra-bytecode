using System.Diagnostics.CodeAnalysis;
using Vectra.AST;
using Vectra.AST.Declarations;
using Vectra.AST.Declarations.Interfaces;
using Vectra.AST.Expressions;
using Vectra.AST.Models;
using Vectra.AST.Statements;
using Vectra.Bytecode.Models;
using Vectra.Bytecode.Tests.TestUtils;

namespace Vectra.Bytecode.Tests;

[ExcludeFromCodeCoverage]
[TestFixture]
public class BytecodeGeneratorTests
{
    [Test]
    public void ShouldCreate()
    {
        var generator = new BytecodeGenerator();
        generator.Should().NotBeNull();
    }

    [Test]
    public void Should_ThrowException_When_NullSpacePassed()
    {
        var module = new VectraASTModuleBuilder().WithName("Test").WithSpace(null!).Build();
        var generator = new BytecodeGenerator();
        generator.Invoking(x => x.Generate(module)).Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Should_ReturnExecutableProgram_When_RootSpaceExists_AND_ModuleIsExecutable()
    {
        var module = new VectraASTModuleBuilder().WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test").Build())
            .WithIsExecutable(true).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        result.Should().NotBeNull();
        result.EntryPointMethod.Should().NotBeNull();
        result.EntryPointMethod.Should().Be("Program.main");
        result.ModuleType.Should().Be(VbcModuleType.Executable);
        result.Dependencies.Should().HaveCount(0);
        result.ModuleName.Should().Be("Test");
    }
    
    [Test]
    public void Should_ReturnLibraryProgram_When_RootSpaceExists_AND_ModuleIsNotExecutable()
    {
        var module = new VectraASTModuleBuilder().WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test").Build())
            .WithIsExecutable(false).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        result.Should().NotBeNull();
        result.EntryPointMethod.Should().BeNull();
        result.ModuleType.Should().Be(VbcModuleType.Library);
    }

    [Test]
    public void Should_HandleNestedSpaces()
    {
        var parentSpace = new SpaceDeclarationNodeBuilder().WithName("Parent").Build();
        new SpaceDeclarationNodeBuilder().WithName("Child").WithParent(parentSpace).Build();
        var module = new VectraASTModuleBuilder().WithName("Test")
            .WithSpace(parentSpace)
            .WithIsExecutable(false).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        result.Should().NotBeNull();
        result.EntryPointMethod.Should().BeNull();
        result.ModuleType.Should().Be(VbcModuleType.Library);
        result.RootSpace.Should().NotBeNull();
        result.RootSpace.Name.Should().Be("Parent");
        result.RootSpace.Subspaces.Should().HaveCount(1);
    }

    [Test]
    public void Should_ThrowArgumentException_When_InvalidDeclarationExists()
    {
        var testClass = new TestClass();
        var module = new VectraASTModuleBuilder().WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([testClass])
                    .Build())
            .WithIsExecutable(false).Build();
        var generator = new BytecodeGenerator();
        generator.Invoking(x => x.Generate(module)).Should().Throw<ArgumentException>();
    }

    [Test]
    public void Should_ReturnAClassInTheRootSpace()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([])
                            .Build()
                    ])
                    .Build())
            .Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        result.Should().NotBeNull();
        result.RootSpace.Should().NotBeNull();
        result.RootSpace.Name.Should().Be("Test");
        result.RootSpace.Types.Should().HaveCount(1);
        result.RootSpace.Types[0].Name.Should().Be("TestClass");
        result.RootSpace.Types[0].Should().BeOfType<VbcClass>();
    }

    [Test]
    public void Should_ReturnMethodWithNoBody_AND_NoParameters()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithIsExecutable(true)
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithName("TestMethod")
                                    .WithReturnType("void").Build()
                            ]).Build()
                    ]).Build())
            .Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var classNode = result.RootSpace.Types[0] as VbcClass;
        classNode.Should().NotBeNull();
        var method = classNode.Methods[0];
        method.Should().NotBeNull();
        method.Instructions.Should().HaveCount(0);
        method.Parameters.Should().HaveCount(0);
        method.Name.Should().Be("TestMethod");
        method.Constants.Should().HaveCount(0);
        method.LocalVariables.Should().HaveCount(1);
    }

    [Test]
    public void Should_ReturnMethodWithNoBody_AND_Parameters()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithName("TestMethod")
                                    .WithReturnType("void")
                                    .WithParameters([
                                        "param1", "param2"
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var classNode = result.RootSpace.Types[0] as VbcClass;
        classNode.Should().NotBeNull();
        var method = classNode.Methods[0];
        method.Should().NotBeNull();
        method.Instructions.Should().HaveCount(0);
        method.Parameters.Should().HaveCount(2);
        method.Name.Should().Be("TestMethod");
        method.Parameters[0].Name.Should().Be("param1");
        method.Parameters[0].TypeName.Should().Be("any");
        method.Parameters[1].Name.Should().Be("param2");
        method.Parameters[1].TypeName.Should().Be("any");
        method.Constants.Should().HaveCount(0);
        method.LocalVariables.Should().HaveCount(3);
    }

    [Test]
    public void Should_HandleReturnStatement_With_NoValue()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithName("TestMethod")
                                    .WithReturnType("void")
                                    .WithBody([
                                        new ReturnStatementNode(null, new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(1);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.Ret);
        instruction.Operand.Should().Be(0);
    }

    [Test]
    public void Should_HandleExpressionStatement_With_LiteralExpression()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithName("TestMethod")
                                    .WithReturnType("void")
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new LiteralExpressionNode(5, new()),
                                            new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(2);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction.Operand.Should().Be(0);
        instruction = method.Instructions[1];
        instruction.OpCode.Should().Be(OpCode.Pop);
    }

    [Test]
    public void Should_HandleReturnStatement_With_Value()
    {
        var module = new VectraASTModuleBuilder()
            .WithName("Test")
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithName("Test")
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithName("TestMethod")
                                    .WithReturnType("void")
                                    .WithBody([
                                        new ReturnStatementNode(new LiteralExpressionNode(5, new()), new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(2);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction.Operand.Should().Be(0);
        instruction = method.Instructions[1];
        instruction.OpCode.Should().Be(OpCode.Ret);
    }

    [Test]
    public void Should_HandleIdentifierExpression_With_PreexistingIdentifier()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new IdentifierExpressionNode("this", new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(1);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadLocal);
        instruction.Operand.Should().Be(0);
    }
    
    [Test]
    public void Should_HandleIdentifierExpression_With_NoPreexistingIdentifier()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new IdentifierExpressionNode("foo", new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(1);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadLocal);
        instruction.Operand.Should().Be(1);
    }

    [Test]
    public void Should_HandleCallExpression_With_NoArguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new CallExpressionNode(
                                            new LiteralExpressionNode("this", new()),
                                            [],
                                            new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(2);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction = method.Instructions[1];
        instruction.OpCode.Should().Be(OpCode.Call);
        instruction.Operand.Should().Be(0);
    }
    
    [Test]
    public void Should_HandleCallExpression_With_Arguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new CallExpressionNode(
                                            new LiteralExpressionNode("this", new()),
                                            [
                                                new LiteralExpressionNode(5, new()),
                                            ],
                                            new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(3);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction = method.Instructions[1];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction.Operand.Should().Be(1);
        instruction = method.Instructions[2];
        instruction.OpCode.Should().Be(OpCode.Call);
        instruction.Operand.Should().Be(0);
    }

    [TestCase("+", OpCode.Add)]
    [TestCase("-", OpCode.Sub)]
    [TestCase("*", OpCode.Mul)]
    [TestCase("/", OpCode.Div)]
    [TestCase("==", OpCode.Eq)]
    [TestCase("!=", OpCode.Neq)]
    [TestCase("<", OpCode.Lt)]
    [TestCase("<=", OpCode.Leq)]
    [TestCase(">", OpCode.Gt)]
    [TestCase(">=", OpCode.Geq)]
    public void Should_HandleBinaryExpression_With_Operator(string op, OpCode expectedOpCode)
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new BinaryExpressionNode(
                                            op,
                                            new LiteralExpressionNode(5, new()),
                                            new LiteralExpressionNode(11, new()),
                                            new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(3);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction = method.Instructions[1];
        instruction.OpCode.Should().Be(OpCode.LoadConst);
        instruction = method.Instructions[2];
        instruction.OpCode.Should().Be(expectedOpCode);
    }

    [Test]
    public void Should_ThrowNotSupportedException_When_InvalidOperatorPassed()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new BinaryExpressionNode(
                                            "e",
                                            new LiteralExpressionNode(5, new()),
                                            new LiteralExpressionNode(11, new()),
                                            new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        generator.Invoking(x => x.Generate(module)).Should().Throw<NotSupportedException>();
    }

    private class TestClass : ITypeDeclarationNode
    {
        public T Accept<T>(IAstVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }

        public SourceSpan Span { get; } = new();
        public string Name { get; } = string.Empty;
    }
}