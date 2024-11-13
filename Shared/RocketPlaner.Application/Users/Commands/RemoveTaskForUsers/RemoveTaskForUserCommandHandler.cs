using System;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;

public class RemoveTaskForUserCommandHandler : ICommandHandler<RemoveTaskForUsersCommand, RocketTask>
{
    private readonly ITaskDataBase Tasks;
    private readonly IUsersDataBase Users;
    public RemoveTaskForUserCommandHandler(ITaskDataBase task, IUsersDataBase user)
    {
        this.Tasks = task;
        this.Users = user;
    }
    public async Task<Result<RocketTask>> Handle(RemoveTaskForUsersCommand command)
    {
        var user = await Users.GetUser(command.TelegrammID);
       if (user == null)
       {
        return new Error("Пользователь с таким id Телеграмма не найден");
       }
       
        var Task =user.Tasks.Find(x=>x.Title == command.Title);
        if(Task.IsError)
        {
            return Task.Error;
        }
        Task = user.Tasks.Remove(Task.Value); 
        await Tasks.Remove(Task.Value);
        return Task;

    }
}
