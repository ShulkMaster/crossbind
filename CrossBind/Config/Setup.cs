using CrossBind.Compiler.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CrossBind.Config;

public static class Setup
{
    public static ServiceProvider Init()
    {
        var ss = new ServiceCollection();
        ss.AddMediatR(Assembly.GetExecutingAssembly());
        ss.AddCrossBindServices();
        RegisterServices(ss);
        ServiceProvider sp = ss.BuildServiceProvider();
        return sp;
    }

    private static void RegisterServices(IServiceCollection sc)
    {
        
    }
}