using CrossBind.Compiler.Typing;
using Microsoft.Extensions.DependencyInjection;

namespace CrossBind.Compiler.Extensions;

public static class DependencyExtension
{
    public static IServiceCollection AddCrossBindServices(this IServiceCollection sc)
    {
        return sc.AddScoped<ITypeManager, TypeManager>();
    }
}