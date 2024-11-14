using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

/// <summary>
/// Интерфейс обработчика операции только для чтения
/// </summary>
/// <typeparam name="TQuery">Операция для чтения</typeparam>
/// <typeparam name="TResult">Результат операции</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Метод обработки операции чтения
    /// </summary>
    /// <param name="query">Операция чтения</param>
    /// <returns>Возвращает результат выполнения операции чтения</returns>
    Task<Result<TResult>> Handle(TQuery query);
}
