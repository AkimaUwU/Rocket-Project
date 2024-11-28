using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public sealed record AddTaskForUsersCommand(
    string? Title,
    string? Message,
    string? Type,
    DateTime? FireDate,
    long? UserTelegramId
) : ICommand<RocketTask>;
