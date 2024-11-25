using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskCommand : ICommand<RocketTaskDestination>
{
    public long? TelegramId { get; init; }
    public string? Title { get; init; }
    public string? ChatId { get; init; }

    public RemoveDestinationFromRocketTaskCommand(long? telegramId, string? title, string? chatId)
    {
        TelegramId = telegramId;
        Title = title;
        ChatId = chatId;
    }
}
