using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface ICommandValidator<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public Task<bool> IsCommandValidAsync(TCommand command);

    public Error GetLastError();
}
