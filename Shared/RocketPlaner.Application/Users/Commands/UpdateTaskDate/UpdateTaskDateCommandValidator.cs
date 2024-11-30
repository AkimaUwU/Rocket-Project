using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public sealed class UpdateTaskDateCommandValidator
    : BaseValidator,
        ICommandValidator<UpdateTaskDateCommand, RocketTask>
{
    public async Task<bool> IsCommandValidAsync(UpdateTaskDateCommand command)
    {
        AddErrorFromResult(UserTelegramId.Create(command.TelegramId));
        AddErrorFromResult(RocketTaskTitle.Create(command.Title));
        AddErrorFromResult(RocketTaskFireDate.Create(command.NewFireDate));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
