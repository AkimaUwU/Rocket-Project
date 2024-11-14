using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommand(long telegramId, string title, DateTime newDateTime)
    : ICommand<RocketTask>
{
    public long TelegramId { get; init; } = telegramId;
    public string Title { get; init; } = title;
    public DateTime NewDate { get; init; } = newDateTime;
}
