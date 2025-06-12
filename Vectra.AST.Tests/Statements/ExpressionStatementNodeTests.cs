using FluentAssertions;
using Vectra.AST.Expressions;
using Vectra.AST.Statements;

namespace Vectra.AST.Tests.Statements;

[TestFixture]
public class ExpressionStatementNodeTests
{
    [Test]
    public void ShouldCreate()
    {
        var node = new ExpressionStatementNode(new LiteralExpressionNode(5, new()), new());
        node.Should().NotBeNull();
        node.Expression.Should().NotBeNull();
        node.Expression.Should().BeOfType<LiteralExpressionNode>();
    }
}