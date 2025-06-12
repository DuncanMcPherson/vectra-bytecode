using FluentAssertions;
using Vectra.AST.Expressions;
using Vectra.AST.Statements;

namespace Vectra.AST.Tests.Statements;

[TestFixture]
public class ReturnStatementNodeTests
{
    [Test]
    public void ShouldCreate_NullValue()
    {
        var node = new ReturnStatementNode(null, new());
        node.Should().NotBeNull();
        node.Value.Should().BeNull();
        node.Span.Should().NotBeNull();
    }

    [Test]
    public void ShouldCreate_NonNullValue()
    {
        var node = new ReturnStatementNode(new LiteralExpressionNode(5, new()), new());
        
        node.Should().NotBeNull();
        node.Value.Should().NotBeNull();
        node.Span.Should().NotBeNull();
    }
}