using CrossBind.Engine.BaseModels;
using Engine.React;

namespace React.EngineTest;

public class UnitTest1
{
    [Fact]
    public void Should_Create_A_Single_Import()
    {
        // arrange
        var cmu = new UnitModel("", "/myfile.txt", new []
        {
            new ImportModel("react", new []{ "xd" }),
        }, ArraySegment<BindModel>.Empty);
        var engine = new ReactEngine();
        
        // act
        var code = engine.CompileUnit(cmu, true);
        
        // aseert
        Assert.Equal("import { xd } from 'react';\n", code);
    }
}