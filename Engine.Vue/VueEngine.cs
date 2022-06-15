using CrossBind.Engine;
using CrossBind.Engine.BaseModels;
using System.Text;

namespace Engine.Vue
{
    public class VueEngine : IEngine
    {
        public string PluginName => "Vue Engine Official";

        public int MajorVersion => 0;

        public int MinorVersion => 1;

        public int PathVersion => 0;

        public EngineTarget Target => EngineTarget.vue;

        public string CompileUnit(UnitModel unit, bool production)
        {
            var stringBuilder = new StringBuilder();

            return stringBuilder.ToString();
        }
    }
}