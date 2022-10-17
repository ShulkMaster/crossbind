using CrossBind.Commands;
using MediatR;

namespace CrossBind.Handlers;

public class CompileHandler: IRequestHandler<Compile, int>
{
    public async Task<int> Handle(Compile request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Compiling the {request.Source}  with {request.PluginId}");
        return 0;
    }
}