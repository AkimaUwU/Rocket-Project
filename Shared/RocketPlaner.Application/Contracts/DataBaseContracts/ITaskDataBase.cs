using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Интерфейс таблицы задач
/// </summary>
public interface ITaskDataBase
{
    /// <summary>
    /// Метод создания записи в таблице задач
    /// </summary>
    /// <param name="task">Модель задачи</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task AddTask(RocketTask task);

    /// <summary>
    /// Метод удаления записи в таблице задач
    /// </summary>
    /// <param name="task">Модель задачи</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task Remove(RocketTask task);

    /// <summary>
    /// Метод изменения данных в записи задачи  в таблице задач
    /// </summary>
    /// <param name="task">Модель задачи</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task Update(RocketTask task);
}
