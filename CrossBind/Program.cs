using CrossBind.Compiler;
using CrossBind.Engine.BaseModels;
using LanguageExt;

namespace CrossBind;

public static class Program
{
    public static void Main()
    {
        _ = FrontCompiler.CompileUnitFile("code/Button.hbt").Match(
            u =>
            {
                var model = u.Models;
                return Unit.Default;
            },
            e =>
            {
                return Unit.Default;
            });
        
    }
}

