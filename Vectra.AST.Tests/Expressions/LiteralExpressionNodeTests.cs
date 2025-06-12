using FluentAssertions;
using Vectra.AST.Expressions;

namespace Vectra.AST.Tests.Expressions;

[TestFixture]
public class LiteralExpressionNodeTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new LiteralExpressionNode(5, new());
        node.Value.Should().Be(5);
    }
}