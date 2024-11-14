namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель для таблицы базы данных - места отправки задач
/// </summary>
public class DestinationsDao
{
    /// <summary>
    /// ИД места отправки задачи
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ИД телеграмм чата
    /// </summary>
    public string ChatId { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное поле - задача для связи 1 * М
    /// </summary>
    public TasksDao Task { get; set; } = null!;

    /// <summary>
    /// Внешний ключ к таблице задач
    /// </summary>
    public Guid TaskId { get; set; }
}
