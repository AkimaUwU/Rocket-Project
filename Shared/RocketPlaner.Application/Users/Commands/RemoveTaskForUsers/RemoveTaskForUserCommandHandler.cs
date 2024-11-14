using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUserCommandHandler(ITaskDataBase tasks, IUsersDataBase users)
    : ICommandHandler<RemoveTaskForUsersCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(RemoveTaskForUsersCommand command)
    {
        var user = await users.GetUser(command.TelegramId);
        if (user == null)
            return new Error("Пользователь с таким id Телеграмма не найден");

        var task = user.Tasks.Find(x => x.Title == command.Title);
        if (task.IsError)
            return task.Error;

        task = user.Tasks.Remove(task);
        await tasks.Remove(task.Value);
        return task;
    }
}
