using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

/// <summary>
/// Интерфейс обработчика комманд
/// </summary>
/// <typeparam name="TCommand">Команда</typeparam>
/// <typeparam name="TResult">Результат выполнения команды</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> Handle(TCommand command);
}
