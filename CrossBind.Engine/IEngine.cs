using CrossBind.Engine.BaseModels;
using CrossBind.Engine.Generated;

namespace CrossBind.Engine;

/// <include file='Docs/Api/IEngine.xml' path='docs/class/*'/>
public interface IEngine
{
    /// <include file='Docs/Api/IEngine.xml' path='docs/Name/*'/>
    public string Name { get; }

    /// <include file='Docs/Api/IEngine.xml' path='docs/CompileUnitFile/*'/>
    public SourceFile[] CompileUnit(UnitModel unit);

}