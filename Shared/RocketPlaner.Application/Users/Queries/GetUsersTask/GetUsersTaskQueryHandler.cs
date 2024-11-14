using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueryHandler(IUsersDataBase users)
    : IQueryHandler<GetUsersTaskQuery, IReadOnlyList<RocketTask>>
{
    public async Task<Result<IReadOnlyList<RocketTask>>> Handle(GetUsersTaskQuery query)
    {
        var user = await users.GetUser(query.TelegramId);
        return user == null
            ? new Error("Пользователь с таким id Телеграмма не найден")
            : Result<IReadOnlyList<RocketTask>>.Success(user.Tasks.GetAll());
    }
}
