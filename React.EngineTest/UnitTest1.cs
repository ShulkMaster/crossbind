using CrossBind.Engine.BaseModels;
using Engine.React;

namespace React.EngineTest;

public class UnitTest1
{
    [Fact]
    public void Should_Create_A_Single_Import()
    {
        // arrange
        List<ImportModel> list = new();
        list.Add(new ImportModel("react", new []{ "xd" }));
        var cmu = new UnitModel("", "/myfile.txt", list, ArraySegment<BindModel>.Empty);
        var engine = new ReactEngine();
        
        // act
        var code = engine.CompileUnit(cmu);
        
        // aseert
        Assert.Equal("import { xd} from 'react';\nimport React from 'react';\r\nimport './myfile.css';\r\n\r\n", code[1].SourceCode);
    }
}