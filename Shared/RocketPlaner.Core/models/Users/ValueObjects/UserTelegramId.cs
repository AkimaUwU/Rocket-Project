using RocketPlaner.Core.models.Users.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users.ValueObjects;

public readonly record struct UserTelegramId
{
    public long TelegramId { get; init; }

    private UserTelegramId(long telegramId) => TelegramId = telegramId;

    public static Result<UserTelegramId> Create(long? telegramId)
    {
        return telegramId switch
        {
            null => UserErrors.CannotCreateWithoutUserTelegramId,
            <= 0 => UserErrors.BadTelegramIdError,
            _ => new UserTelegramId(telegramId.Value),
        };
    }
}
