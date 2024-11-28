namespace RocketPlaner.Application.Contracts.Operations;

/// <summary>
/// Интерфейс для операций связанных с изменением состояния системы
/// </summary>
/// <typeparam name="TResult">Результат выполнения команды</typeparam>
public interface ICommand<TResult>;
