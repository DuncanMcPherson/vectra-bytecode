using FluentAssertions;
using Vectra.AST.Declarations;
using Vectra.AST.Models;

namespace Vectra.AST.Tests.Declarations;

[TestFixture]
public class MethodDeclarationNodeTests
{
    [Test]
    public void ShouldProperlyCreate()
    {
        var node = new MethodDeclarationNode("Test", [], [], new SourceSpan(0, 0, 3, 1), "void");
        node.Should().NotBeNull();
        node.Name.Should().Be("Test");
        node.Parameters.Should().HaveCount(0);
        node.Body.Should().HaveCount(0);
        node.Span.Should().NotBeNull();
        node.ReturnType.Should().Be("void");
    }
}