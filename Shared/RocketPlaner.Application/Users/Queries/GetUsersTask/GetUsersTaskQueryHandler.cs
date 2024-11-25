using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueryHandler(IUsersDataBase users)
    : IQueryHandler<GetUsersTaskQuery, IReadOnlyList<RocketTask>>
{
    public async Task<Result<IReadOnlyList<RocketTask>>> Handle(GetUsersTaskQuery query)
    {
        var userDao = await users.GetUser(query.TelegramId);
        if (userDao is null)
            return UserErrors.UserNotFound;

        var user = userDao.ToUser();
        var tasks = user.Tasks.GetAll();
        return Result<IReadOnlyList<RocketTask>>.Success(tasks);
    }
}
