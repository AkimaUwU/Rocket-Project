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
        var userDao = await users.GetUser(command.TelegramId);
        if (userDao == null)
            return new Error("Пользователь с таким id Телеграмма не найден");

        var user = userDao.ToUser();
        var task = user.Tasks.Find(t => t.Title == command.Title);
        if (task.IsError)
            return task;

        task = user.Tasks.Remove(task);
        await tasks.Remove(task.Value.ToRocketTaskDao(userDao));
        return task;
    }
}
