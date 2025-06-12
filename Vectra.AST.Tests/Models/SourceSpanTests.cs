using FluentAssertions;
using Vectra.AST.Models;

namespace Vectra.AST.Tests.Models;

[TestFixture]
public class SourceSpanTests
{
    [Test]
    public void ShouldCreate()
    {
        var span = new SourceSpan(1, 2, 3, 4);
        span.Should().NotBeNull();
        span.EndColumn.Should().Be(4);
        span.EndLine.Should().Be(3);
        span.StartColumn.Should().Be(2);
        span.StartLine.Should().Be(1);
    }
}