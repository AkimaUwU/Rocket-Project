using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUsersCommand(long telegramId, string title) : ICommand<RocketTask>
{
    public long TelegramId { get; init; } = telegramId;
    public string Title { get; init; } = title;
}
