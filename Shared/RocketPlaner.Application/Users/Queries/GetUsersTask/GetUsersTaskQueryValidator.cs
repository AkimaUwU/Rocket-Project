using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public sealed class GetUsersTaskQueryValidator
    : BaseValidator,
        IQueryValidator<GetUsersTaskQuery, IReadOnlyList<RocketTask>>
{
    public async Task<bool> IsQueryValidAsync(GetUsersTaskQuery query)
    {
        var telegramId = UserTelegramId.Create(query.TelegramId);
        if (telegramId.IsError)
            errors.Add(telegramId.Error);

        return await Task.FromResult(errors.Count == 0);
    }

    public Error GetLastError() => LastError;
}
