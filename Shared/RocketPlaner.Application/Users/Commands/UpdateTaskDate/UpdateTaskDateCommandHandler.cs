using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommandHandler(ITaskDataBase tasks, IUsersDataBase users)
    : ICommandHandler<UpdateTaskDateCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(UpdateTaskDateCommand command)
    {
        var userDao = await users.GetUser(command.TelegramId);
        if (userDao == null)
            return new Error("Пользователь с таким id Телеграмма не найден");

        var user = userDao.ToUser();
        var task = user.Tasks.Find(x => x.Title == command.Title);
        if (task.IsError)
            return task.Error;

        task = task.Value.UpdateNotificationDate(command.NewDate);
        if (task.IsError)
            return task.Error;

        await tasks.Update(task.Value.ToRocketTaskDao(userDao));
        return task;
    }
}
