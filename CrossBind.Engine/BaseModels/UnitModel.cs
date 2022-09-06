namespace CrossBind.Engine.BaseModels;

public record UnitModel(
    string ModuleHash,
    string FilePath,
    List<ImportModel> Modules,
    IEnumerable<BindModel> Models
);