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
        var telegramId = UserTelegramId.Create(command.TelegramId);
        if (telegramId.IsError)
            errors.Add(telegramId.Error);

        var title = RocketTaskTitle.Create(command.Title);
        if (title.IsError)
            errors.Add(title.Error);

        var date = RocketTaskFireDate.Create(command.NewFireDate);
        if (date.IsError)
            errors.Add(date.Error);

        return await Task.FromResult(errors.Count == 0);
    }

    public Error GetLastError() => LastError;
}
