using System.Text;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks;

/// <summary>
/// Доменная сущность - запланированная задача
/// </summary>
public class RocketTask : DomainAggregateRoot
{
    /// <summary>
    /// Создатель задачи
    /// </summary>
    public User Owner { get; private set; }

    /// <summary>
    /// Дата создания запланиированной задачи
    /// </summary>
    public DateTime CreatedDate { get; init; }

    /// <summary>
    /// Дата уведомления пользователя о запланированной задаче
    /// </summary>
    public DateTime NotifyDate { get; private set; }

    /// <summary>
    /// Сообщение уведомления
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Тип запланированной задачи
    /// </summary>
    public RocketTaskType Type { get; private set; }

    /// <summary>
    /// Список чатов, куда задача будет отправляться
    /// </summary>
    public RocketTaskDestinationList Destinations { get; init; } = new RocketTaskDestinationList();

    /// <summary>
    /// Заголовок задачи
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Конструктор создания экземпляра класса RocketTask
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <param name="message">Сообщение задачи</param>
    /// <param name="user">Обладатель задачи</param>
    /// <param name="type">Тип задачи</param>
    /// <param name="notifyTime">Время уведомления задачи</param>
    /// <param name="title">Заголовок задачи</param>
    private RocketTask(
        Guid id,
        string message,
        User user,
        RocketTaskType type,
        DateTime notifyTime,
        string title
    )
        : base(id)
    {
        Owner = user;
        Message = message;
        Type = type;
        NotifyDate = notifyTime;
        CreatedDate = DateTime.Now;
        Title = title;
    }

    /// <summary>
    /// Статичный фабричный метод создания экземпляра задачи
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <param name="message">Сообщение задачи</param>
    /// <param name="user">Пользователь обладатель задачи</param>
    /// <param name="type">Тип задачи</param>
    /// <param name="notifyTime">Время уведомления о задаче</param>
    /// <param name="title">Заголовок задачи</param>
    /// <returns>Экземпляр класса RocketTask. Успешен/Ошибка</returns>
    public static Result<RocketTask> Create(
        Guid id,
        string message,
        User? user,
        string type,
        DateTime notifyTime,
        string title
    )
    {
        if (user == null)
            return new Error("Задача не может быть создана без пользователя");

        if (string.IsNullOrWhiteSpace(message))
            return new Error("Сообщение не может быть пустым");

        if (notifyTime < DateTime.Now)
            return new Error(
                "Некорректная дата уведомления задачи. Дата не должна быть ранее текущей даты"
            );

        if (notifyTime == DateTime.Now)
            return new Error(
                "Некорректное время уведомления задачи. Время уведомления задачи не может быть текущим"
            );

        RocketTaskType? requestedType = type switch
        {
            "Одноразовая" => new OneLife(),
            "Повторяющаяся" => new NoOneLife(),
            _ => null,
        };
        if (requestedType == null)
            return new Error("Некорректный тип задачи");

        RocketTask task = new RocketTask(id, message, user, requestedType, notifyTime, title);
        return task;
    }

    /// <summary>
    /// Метод обновления времени для задачи
    /// </summary>
    /// <param name="updated">Новое время уведомления задачи</param>
    /// <returns>Результат обновления даты уведомления о задаче. Успешно / неуспешно</returns>
    public Result<RocketTask> UpdateNotificationDate(DateTime updated)
    {
        DateTime current = DateTime.Now;
        if (updated <= current)
            return new Error("Нельзя задать дату уведомления о задаче для прошедшего времени");

        NotifyDate = updated;
        return this;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"ИД задачи: {Id}");
        builder.AppendLine($"Заголовок задачи: {Title}");
        builder.AppendLine($"Текст задачи: {Message}");
        builder.AppendLine($"Дата создания задачи: {CreatedDate.ToString("dd/MM/yyyy")}");
        builder.AppendLine($"Дата удаления задачи: {NotifyDate.ToString("dd/MM/yyyy")}");
        builder.AppendLine($"Тип задачи: {Type.Type}");
        return builder.ToString();
    }
}