using FluentAssertions;
using Vectra.AST.Declarations;
using Vectra.AST.Models;

namespace Vectra.AST.Tests.Declarations;

[TestFixture]
public class ClassDeclarationNodeTests
{
    [Test]
    public void Should_Create_Class_Declaration_Node()
    {
        var node = new ClassDeclarationNode("Test node", [], new SourceSpan(0, 0, 3, 1));
        node.Should().NotBeNull();
        node.Name.Should().Be("Test node");
        node.Members.Should().HaveCount(0);
        node.Span.Should().NotBeNull();
    }
}