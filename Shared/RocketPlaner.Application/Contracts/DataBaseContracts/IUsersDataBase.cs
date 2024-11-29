using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public interface IUsersDataBase
{
    Task AddUser(User user);
    Task RemoveUser(User user);
    Task<User?> GetUser(UserTelegramId telegramId);
    Task<bool> EnsureTelegramIdIsUnique(UserTelegramId telegramId);
    Task UpdateUser(User user);
}
