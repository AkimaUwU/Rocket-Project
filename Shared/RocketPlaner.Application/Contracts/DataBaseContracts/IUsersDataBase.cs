using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public interface IUsersDataBase
{
    Task AddUser(User user);
    Task RemoveUser(User user);
    Task GetUser(long telegramId);
}
