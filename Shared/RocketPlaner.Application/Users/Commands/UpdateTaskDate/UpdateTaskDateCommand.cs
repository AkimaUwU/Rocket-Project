using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public record UpdateTaskDateCommand(long? TelegramId, string Title, DateTime NewFireDate)
    : ICommand<RocketTask>;
