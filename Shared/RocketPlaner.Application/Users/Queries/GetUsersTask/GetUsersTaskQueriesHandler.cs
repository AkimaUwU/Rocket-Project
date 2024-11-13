using System;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueriesHandler:IQueriesHandler<GetUsersTaskQueries,IReadOnlyList<RocketTask>>
{
    private readonly IUsersDataBase Users;
    public GetUsersTaskQueriesHandler(IUsersDataBase users)
    {
        this.Users=users;   
    }

    public async Task<Result<IReadOnlyList<RocketTask>>> Handle(GetUsersTaskQueries command)
    {
       var user = await Users.GetUser(command.TelegrammID);
       if (user == null)
       {
        return new Error("Пользователь с таким id Телеграмма не найден");
       }
       return Result<IReadOnlyList<RocketTask>>.Success (user.Tasks.GetAll());
       
    }
}
