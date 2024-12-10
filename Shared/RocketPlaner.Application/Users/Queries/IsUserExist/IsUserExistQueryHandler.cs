using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Users.Queries.IsUserExist;

internal sealed class IsUserExistQueryHandler(
    IUsersDataBase dataBase,
    IQueryValidator<IsUserExistQuery, bool> validator
) : IQueryHandler<IsUserExistQuery, bool>
{
    public async Task<Result<bool>> Handle(IsUserExistQuery query)
    {
        if (!await validator.IsQueryValidAsync(query))
            return validator.GetLastError();

        UserTelegramId telegramId = UserTelegramId.Create(query.TelegramId);
        bool isExists = await dataBase.EnsureTelegramIdIsUnique(telegramId);
        return !isExists
            ? true
            : new Error(
                "Необходимо пройти процедуру регистрации.\nКоманда: \"Зарегистрироваться\""
            );
    }
}
