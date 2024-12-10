using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceScopeFactory _factory;

    public CommandDispatcher(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task<Result<TCommandResult>> Resolve<TCommand, TCommandResult>(TCommand command)
        where TCommand : ICommand<TCommandResult>
    {
        using (var scope = _factory.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<
                ICommandHandler<TCommand, TCommandResult>
            >();
            return await handler.Handle((TCommand)command);
        }
    }
}
