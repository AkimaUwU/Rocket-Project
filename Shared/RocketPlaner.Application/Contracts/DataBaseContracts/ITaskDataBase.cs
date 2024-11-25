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
    Task AddTask(TasksDao task);

    /// <summary>
    /// Метод удаления записи в таблице задач
    /// </summary>
    /// <param name="task">Модель задачи</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task Remove(TasksDao task);

    /// <summary>
    /// Метод изменения данных в записи задачи  в таблице задач
    /// </summary>
    /// <param name="task">Модель задачи</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task Update(TasksDao task);
}
