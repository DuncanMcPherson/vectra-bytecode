using FluentAssertions;
using Vectra.AST.Expressions;

namespace Vectra.AST.Tests.Expressions;

[TestFixture]
public class CallExpressionNodeTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new CallExpressionNode(new LiteralExpressionNode(5, new()), [], new());
        node.Should().NotBeNull();
        node.Target.Should().NotBeNull();
        node.Arguments.Should().HaveCount(0);
    }
}