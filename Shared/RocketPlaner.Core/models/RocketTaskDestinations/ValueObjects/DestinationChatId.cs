using RocketPlaner.Core.models.RocketTaskDestinations.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;

public readonly record struct DestinationChatId
{
    public long ChatId { get; init; }

    private DestinationChatId(long chatId) => ChatId = chatId;

    public static Result<DestinationChatId> Create(long? chatId)
    {
        return chatId switch
        {
            null => RocketTaskDestinationErrors.CannotCreateWithoutChatId,
            <= 0 => RocketTaskDestinationErrors.CannotCreateWithInvalidChatId,
            _ => new DestinationChatId(chatId.Value),
        };
    }
}
