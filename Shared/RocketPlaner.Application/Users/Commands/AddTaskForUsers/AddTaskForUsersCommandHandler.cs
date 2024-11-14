using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommandHandler(IUsersDataBase users, ITaskDataBase tasks)
    : ICommandHandler<AddTaskForUsersCommand, RocketTask>
{
    public async Task<Result<RocketTask>> Handle(AddTaskForUsersCommand command)
    {
        var user = await users.GetUser(command.IdUser);
        if (user == null)
            return new Error("Пользователь с таким id Телеграмма не найден");

        var task = RocketTask.Create(
            command.Message,
            command.Destination,
            user,
            command.TypeTask,
            command.NotifyDate,
            command.TitleTask
        );

        if (task.IsError)
            return task.Error;

        await tasks.AddTask(task.Value);
        return task;
    }
}
