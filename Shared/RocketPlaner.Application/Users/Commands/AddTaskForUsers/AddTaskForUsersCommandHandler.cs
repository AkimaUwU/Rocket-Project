using System;
using System.Net.WebSockets;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommandHandler : ICommandHandler<AddTaskForUsersCommand, RocketTask>
{
    private readonly ITaskDataBase Tasks;
    private readonly IUsersDataBase Users;
    public AddTaskForUsersCommandHandler(IUsersDataBase users,ITaskDataBase tasks)
    {
        this.Users = users;
        this.Tasks = tasks;
    }
    public async Task<Result<RocketTask>> Handle(AddTaskForUsersCommand command)
    {
       var user = await Users.GetUser(command.IdUser);
       if (user == null)
       {
        return new Error("Пользователь с таким id Телеграмма не найден");
       }
       
        var Task = RocketTask.Create(command.Messege,command.Destination,user,command.TypeTask,command.NotifyDate,command.TitleTask);
        if(Task.IsError)
        {
            return Task.Error;
        }
        await Tasks.AddTask(Task.Value);
        return Task;
    }
}
