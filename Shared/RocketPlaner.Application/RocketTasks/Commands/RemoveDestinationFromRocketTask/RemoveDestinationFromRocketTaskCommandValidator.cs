using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;

public sealed class RemoveDestinationFromRocketTaskCommandValidator
    : BaseValidator,
        ICommandValidator<RemoveDestinationFromRocketTaskCommand, RocketTaskDestination>
{
    public async Task<bool> IsCommandValidAsync(RemoveDestinationFromRocketTaskCommand command)
    {
        AddErrorFromResult(UserTelegramId.Create(command.UserTelegramId));
        AddErrorFromResult(RocketTaskTitle.Create(command.Title));
        AddErrorFromResult(DestinationChatId.Create(command.ChatId));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
