using FluentAssertions;
using Vectra.AST.Expressions;

namespace Vectra.AST.Tests.Expressions;

[TestFixture]
public class IdentifierExpressionNodeTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new IdentifierExpressionNode("Test", new());

        node.Should().NotBeNull();
        node.Name.Should().Be("Test");
        node.Span.Should().NotBeNull();
    }
}