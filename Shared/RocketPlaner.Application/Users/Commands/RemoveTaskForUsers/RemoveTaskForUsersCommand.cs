using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public record RemoveTaskForUsersCommand(string? Title, long? UserTelegramId) : ICommand<RocketTask>;
