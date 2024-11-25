using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface ICommandDispatcher
{
    Task<Result<TCommandResult>> Resolve<TCommand, TCommandResult>(TCommand command)
        where TCommand : ICommand<TCommandResult>;
}
