namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель таблицы задача в базе данных
/// </summary>
public class TasksDao
{
    /// <summary>
    /// Ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний ключ на таблицу пользователя
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Навигационное свойство пользователь для связи 1 * М
    /// </summary>
    public UsersDao Owner { get; set; } = null!;

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Дата уведомления о задаче
    /// </summary>
    public DateTime NotifyDate { get; set; }

    /// <summary>
    /// Тип задачи
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение задачи
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Заголовок задачи
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное свойство список мест отправки задачи для связи 1 * М
    /// </summary>
    public List<DestinationsDao> Destinations { get; set; } = [];
}
