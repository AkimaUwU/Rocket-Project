using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users;

public sealed class User : DomainAggregateRoot
{
    public UserTasksList Tasks { get; init; } = [];

    public long TelegramId { get; init; }

    private User(long telegramId, Guid id)
        : base(id) => TelegramId = telegramId;

    public static Result<User> Create(long telegramId)
    {
        if (telegramId <= 0)
            return new Error("ID телеграмма пользователя некорректно");

        return new User(telegramId, Guid.NewGuid());
    }
}
