namespace CrossBind.Model;

public record UnitModel(
    string ModuleHash,
    string FilePath,
    IEnumerable<ImportModel> Modules,
    IEnumerable<BindModel> Models
);