namespace CrossBind.Compiler.Test.CompileTest;

public class FrontCompilerTest
{
    [Fact]
    public void Should_Compile_Valid_Basic_Button()
    {
        var unit = FrontCompiler.CompileUnitFile("code/Button.hbt");
        Assert.True(unit.IsSuccess);
    }
}