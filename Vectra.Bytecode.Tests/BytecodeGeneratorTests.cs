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
                                        new Parameter("param1", "any"), new Parameter("param2", "any")
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
                                        new CallExpressionNode(
                                            new IdentifierExpressionNode("foo", new()), [], "test", new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(2);
        var instruction = method.Instructions[0];
        instruction.OpCode.Should().Be(OpCode.LoadLocal);
        instruction.Operand.Should().Be(1);
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
                                        new CallExpressionNode(
                                            new IdentifierExpressionNode("foo", new()),
                                            [],
                                            "test",
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
                                            "testMethod",
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
        instruction.Operand.Should().Be(1);
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
                                            "testMethod",
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
        var methodIndex = instruction.Operand & 0x00FFFFFF;
        methodIndex.Should().Be(2);
        var arity = (instruction.Operand >> 24) & 0xFF;
        arity.Should().Be(1);
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
                                        new ExpressionStatementNode(
                                        new BinaryExpressionNode(
                                            op,
                                            new LiteralExpressionNode(5, new()),
                                            new LiteralExpressionNode(11, new()),
                                            new()), new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        method.Instructions.Should().HaveCount(4);
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
                                        new ExpressionStatementNode(
                                        new BinaryExpressionNode(
                                            "e",
                                            new LiteralExpressionNode(5, new()),
                                            new LiteralExpressionNode(11, new()),
                                            new()),
                                        new())
                                    ]).Build()
                            ]).Build()
                    ]).Build()
            ).Build();
        var generator = new BytecodeGenerator();
        generator.Invoking(x => x.Generate(module)).Should().Throw<NotSupportedException>();
    }

    [Test]
    public void Should_HandleVariableDeclaration_Without_Initializer()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new VariableDeclarationNode(
                                            "testVar", 
                                            "any", 
                                            null, 
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(2);
        method.LocalVariables.Should().Contain("testVar");
        
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadDefault);
        method.Instructions[1].OpCode.Should().Be(OpCode.StoreLocal);
        method.Instructions[1].Operand.Should().Be(1); // Index 1 because "this" is at index 0
    }

    [Test]
    public void Should_HandleVariableDeclaration_With_Initializer()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new VariableDeclarationNode(
                                            "testVar", 
                                            "any", 
                                            new LiteralExpressionNode(42, new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(2);
        method.LocalVariables.Should().Contain("testVar");
        
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[0].Operand.Should().Be(0); // First constant
        method.Instructions[1].OpCode.Should().Be(OpCode.StoreLocal);
        method.Instructions[1].Operand.Should().Be(1); // Index 1 because "this" is at index 0
    }

    [Test]
    public void Should_HandleMultipleVariableDeclarations_With_CorrectIndices()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new VariableDeclarationNode("var1", "any", null, new()),
                                        new VariableDeclarationNode("var2", "any", null, new())
                                    ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(4);
        method.LocalVariables.Should().HaveCount(3); // "this" + var1 + var2
        method.LocalVariables.Should().ContainInOrder(["this", "var1", "var2"]);
        
        // First variable
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadDefault);
        method.Instructions[1].OpCode.Should().Be(OpCode.StoreLocal);
        method.Instructions[1].Operand.Should().Be(1);
        
        // Second variable
        method.Instructions[2].OpCode.Should().Be(OpCode.LoadDefault);
        method.Instructions[3].OpCode.Should().Be(OpCode.StoreLocal);
        method.Instructions[3].Operand.Should().Be(2);
    }

    [Test]
    public void Should_HandleVariableDeclaration_With_ComplexInitializer()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new VariableDeclarationNode(
                                            "result",
                                            "any",
                                            new BinaryExpressionNode(
                                                "+",
                                                new LiteralExpressionNode(5, new()),
                                                new LiteralExpressionNode(3, new()),
                                                new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(4);
        method.LocalVariables.Should().Contain("result");
        
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[1].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[2].OpCode.Should().Be(OpCode.Add);
        method.Instructions[3].OpCode.Should().Be(OpCode.StoreLocal);
        method.Instructions[3].Operand.Should().Be(1);
    }

    [Test]
    public void Should_HandleNewExpression_Without_Arguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new NewExpressionNode("TestClass", [], new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(2); // New + Pop
        method.Instructions[0].OpCode.Should().Be(OpCode.New);
        method.Instructions[0].Operand.Should().Be(0); // Type index 0, 0 arguments
    }

    [Test]
    public void Should_HandleNewExpression_With_Arguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new NewExpressionNode(
                                                "TestClass",
                                                [new LiteralExpressionNode(42, new())],
                                                new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(3); // LoadConst + New + Pop
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[1].OpCode.Should().Be(OpCode.New);
        
        var operand = method.Instructions[1].Operand;
        var argCount = (operand >> 24) & 0xFF;
        var typeIndex = operand & 0x00FFFFFF;
        
        argCount.Should().Be(1);
        typeIndex.Should().Be(0);
    }

    [Test]
    public void Should_ThrowException_When_TypeNotFound()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new NewExpressionNode(
                                                "UnknownClass",
                                                [],
                                                new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        generator.Invoking(x => x.Generate(module))
            .Should().Throw<Exception>()
            .WithMessage("Unknown type 'UnknownClass'");
    }

    [Test]
    public void Should_HandleNewExpression_With_MultipleArguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new NewExpressionNode(
                                                "TestClass",
                                                [
                                                    new LiteralExpressionNode(1, new()),
                                                    new LiteralExpressionNode(2, new()),
                                                    new LiteralExpressionNode(3, new())
                                                ],
                                                new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(5); // 3 LoadConst + New + Pop
        method.Instructions[3].OpCode.Should().Be(OpCode.New);
        
        var operand = method.Instructions[3].Operand;
        var argCount = (operand >> 24) & 0xFF;
        var typeIndex = operand & 0x00FFFFFF;
        
        argCount.Should().Be(3);
        typeIndex.Should().Be(0);
    }

    [Test]
    public void Should_HandleNewExpression_With_ComplexArguments()
    {
        var module = new VectraASTModuleBuilder()
            .WithSpace(
                new SpaceDeclarationNodeBuilder()
                    .WithDeclarations([
                        new ClassDeclarationNodeBuilder()
                            .WithName("TestClass")
                            .WithMembers([
                                new MethodDeclarationNodeBuilder()
                                    .WithBody([
                                        new ExpressionStatementNode(
                                            new NewExpressionNode(
                                                "TestClass",
                                                [
                                                    new BinaryExpressionNode(
                                                        "+",
                                                        new LiteralExpressionNode(1, new()),
                                                        new LiteralExpressionNode(2, new()),
                                                        new())
                                                ],
                                                new()),
                                            new())
                                ]).Build()
                        ]).Build()
                ]).Build()
        ).Build();

        var generator = new BytecodeGenerator();
        var result = generator.Generate(module);
        var method = (result.RootSpace.Types[0] as VbcClass)!.Methods[0];
        
        method.Instructions.Should().HaveCount(5); // 2 LoadConst + Add + New + Pop
        
        method.Instructions[0].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[1].OpCode.Should().Be(OpCode.LoadConst);
        method.Instructions[2].OpCode.Should().Be(OpCode.Add);
        method.Instructions[3].OpCode.Should().Be(OpCode.New);
        
        var operand = method.Instructions[3].Operand;
        var argCount = (operand >> 24) & 0xFF;
        var typeIndex = operand & 0x00FFFFFF;
        
        argCount.Should().Be(1);
        typeIndex.Should().Be(0);
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