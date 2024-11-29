using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueryHandler(
    IUsersDataBase users,
    IQueryValidator<GetUsersTaskQuery, IReadOnlyList<RocketTask>> validator
) : IQueryHandler<GetUsersTaskQuery, IReadOnlyList<RocketTask>>
{
    public async Task<Result<IReadOnlyList<RocketTask>>> Handle(GetUsersTaskQuery query)
    {
        if (!await validator.IsQueryValidAsync(query))
            return validator.GetLastError();
        var userTelegramId = UserTelegramId.Create(query.TelegramId);
        var user = await users.GetUser(userTelegramId);
        return user is null ? [] : user.Tasks.ToList();
    }
}
