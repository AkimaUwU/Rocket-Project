using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQueryHandler : IQueryHandler<GetUsersTaskQuery, IReadOnlyList<RocketTask>>
{
    private readonly IUsersDataBase _users;

    public GetUsersTaskQueryHandler(IUsersDataBase users)
    {
        _users = users;
    }

    public async Task<Result<IReadOnlyList<RocketTask>>> Handle(GetUsersTaskQuery query)
    {
        var userTelegramId = UserTelegramId.Create(query.TelegramId);
        if (userTelegramId.IsError)
            return new List<RocketTask>();

        var user = await _users.GetUser(userTelegramId);
        return user is null ? [] : user.Tasks.ToList();
    }
}
