using FluentAssertions;
using Vectra.AST.Declarations;
using Vectra.AST.Models;

namespace Vectra.AST.Tests.Declarations;

[TestFixture]
public class SpaceDeclarationNodeTests
{
    [Test]
    public void Should_Create_Space_Declaration_Node()
    {
        var node = new SpaceDeclarationNode("Test", [], new SourceSpan(), null);
        node.Should().NotBeNull();
        node.Name.Should().Be("Test");
        node.ChildSpaces.Should().HaveCount(0);
        node.QualifiedName.Should().Be("Test");
        node.Declarations.Should().HaveCount(0);
        node.Span.Should().NotBeNull();
    }

    [Test]
    public void Should_CreateSpaceWithParentNode()
    {
        var parent = new SpaceDeclarationNode("Test", [], new SourceSpan(), null);
        var node = new SpaceDeclarationNode("Child", [], new SourceSpan(), parent);
        parent.ChildSpaces.Should().HaveCount(1);
        parent.ChildSpaces[0].Should().Be(node);
        node.QualifiedName.Should().Be("Test.Child");
    }
}