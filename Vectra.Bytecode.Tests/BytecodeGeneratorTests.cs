namespace Vectra.Bytecode.Tests;

[TestFixture]
public class BytecodeGeneratorTests
{
    [Test]
    public void ShouldCreate()
    {
        var generator = new BytecodeGenerator();
        generator.Should().NotBeNull();
    }
}