using FluentAssertions;
using Vectra.AST.Declarations;

namespace Vectra.AST.Tests;

[TestFixture]
public class VectraASTModuleTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new VectraASTModule("Test", new SpaceDeclarationNode("Test", [], new(), null));
        node.Should().NotBeNull();
        node.IsExecutable.Should().BeTrue();
        node.Name.Should().Be("Test");
        node.RootSpace.Should().NotBeNull();
        node.IsExecutable = false;
        node.IsExecutable.Should().BeFalse();
    }
}