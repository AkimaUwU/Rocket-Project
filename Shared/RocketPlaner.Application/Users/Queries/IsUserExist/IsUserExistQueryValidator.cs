using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.IsUserExist;

public sealed class IsUserExistQueryValidator
    : BaseValidator,
        IQueryValidator<IsUserExistQuery, bool>
{
    public async Task<bool> IsQueryValidAsync(IsUserExistQuery query)
    {
        AddErrorFromResult(UserTelegramId.Create(query.TelegramId));
        return await Task.FromResult(HasErrors);
    }

    public Error GetLastError() => LastError;
}
