using CrossBind.Engine;
using CrossBind.Engine.BaseModels;
using CrossBind.Engine.ComponentModels;
using CrossBind.Engine.Generated;
using System.Text;

namespace Engine.Vue
{
    public class VueEngine : IEngine
    {
        public string PluginId => "Vue.Official";

        public string PluginName => "Vue Engine Official";

        public int MajorVersion => 0;

        public int MinorVersion => 1;

        public int PathVersion => 0;

        public EngineTarget Target => EngineTarget.Vue;

        private static SourceFile CompileComponent(ComponentModel model, StringBuilder sb)
        {
            string TemplateTag = "template";
            string ScriptTag = "script";
            string StyleTag = "style";

            sb.Append($"<{TemplateTag}>\n");

            string tagIncome = DomVueTypes.GetComponentTag(model.Extends);

            sb.Append($"\t<{tagIncome} props=\"\">\n");
            sb.Append($"</{TemplateTag}>");

            return new SourceFile(model.Name, "vue");

        }

        private static SourceFile CompileModel(BindModel model, StringBuilder sb)
        {
            switch (model)
            {
                case ComponentModel compModel:
                    {
                        return CompileComponent(compModel, sb);
                    }
            }

            return new SourceFile("", "");
        }

        public SourceFile[] CompileUnit(UnitModel unit, bool production)
        {
            var sb = new StringBuilder();
            string baseName = Path.GetFileName(unit.FilePath);
            int dotIndex = baseName.IndexOf('.');
            string fileName = baseName[..dotIndex];

            var files = new List<SourceFile>();

            foreach (var model in unit.Models)
            {
                var source = CompileModel(model, sb);
                files.Add(source);
            }

            return files.ToArray();
        }
    }
}