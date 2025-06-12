using FluentAssertions;
using Vectra.AST.Expressions;
using Vectra.AST.Models;

namespace Vectra.AST.Tests.Expressions;

[TestFixture]
public class BinaryExpressionNodeTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new BinaryExpressionNode("+", new LiteralExpressionNode(0, new SourceSpan()),
            new LiteralExpressionNode(5, new SourceSpan()), new SourceSpan());
        node.Should().NotBeNull();
        node.Left.Should().NotBeNull();
        node.Left.Should().BeOfType<LiteralExpressionNode>();
        node.Right.Should().NotBeNull();
        node.Right.Should().BeOfType<LiteralExpressionNode>();
        node.Operator.Should().Be("+");
        node.Span.Should().NotBeNull();
    }
}