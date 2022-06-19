using CrossBind.Compiler.Test.Samples;

namespace CrossBind.Compiler.Test.CompileTest;

public class FrontCompilerTest
{
    [Fact]
    public void Should_Compile_Valid_Basic_Button()
    {
        var unit = FrontCompiler.CompileUnitFile(UnitSample.BasicUnitSample);
        Assert.True(unit.IsSuccess);
    }
}