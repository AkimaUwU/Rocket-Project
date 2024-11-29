using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;

public sealed class AddDestinationForRocketTaskCommandValidator
    : BaseValidator,
        ICommandValidator<AddDestinationForRocketTaskCommand, RocketTaskDestination>
{
    public async Task<bool> IsCommandValidAsync(AddDestinationForRocketTaskCommand command)
    {
        AddErrorFromResult(UserTelegramId.Create(command.UserTelegramId));
        AddErrorFromResult(RocketTaskTitle.Create(command.Title));
        AddErrorFromResult(DestinationChatId.Create(command.ChatId));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
