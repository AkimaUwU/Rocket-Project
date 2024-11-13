using System;
using System.Security.AccessControl;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Commands.UpdateTaskDate;

public class UpdateTaskDateCommandHandler : ICommandHandler<UpdateTaskDateCommand, RocketTask>
{
    private readonly ITaskDataBase Tasks;
    private readonly IUsersDataBase Users;
    public UpdateTaskDateCommandHandler(ITaskDataBase tasks, IUsersDataBase users)
    {
        this.Tasks = tasks;
        this.Users = users;
    }
    public async Task<Result<RocketTask>> Handle(UpdateTaskDateCommand command)
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
        Task = Task.Value.UpdateNotificationDate(command.NewDate);
        if(Task.IsError)
        {
            return Task.Error;
        }
        await Tasks.Update(Task.Value);
        return Task;
    }
}
