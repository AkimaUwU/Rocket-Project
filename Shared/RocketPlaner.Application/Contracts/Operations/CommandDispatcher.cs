using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TCommandResult>> Resolve<TCommand, TCommandResult>(TCommand command)
        where TCommand : ICommand<TCommandResult>
    {
        var handler = _serviceProvider.GetRequiredService<
            ICommandHandler<TCommand, TCommandResult>
        >();
        return await handler.Handle(command);
    }
}
