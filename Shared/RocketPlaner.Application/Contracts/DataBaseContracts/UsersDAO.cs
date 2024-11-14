namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель таблицы пользователи в базе данных
/// </summary>
public sealed class UsersDao
{
    /// <summary>
    /// Ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ИД телеграмма пользователя
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Навигационное свойство список задач для связи 1 * М
    /// </summary>
    public List<TasksDao> Tasks { get; set; } = [];
}
