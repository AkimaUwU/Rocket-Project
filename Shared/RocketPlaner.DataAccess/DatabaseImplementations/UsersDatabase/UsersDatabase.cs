using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

public sealed class UsersDatabase : IUsersDataBase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task AddUser(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task RemoveUser(User user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task<User?> GetUser(UserTelegramId telegramId) =>
        await _context
            .Users.Include(u => u.Tasks)
            .ThenInclude(t => t.Destinations)
            .FirstOrDefaultAsync(u => u.TelegramId.TelegramId == telegramId.TelegramId);

    public async Task<bool> EnsureTelegramIdIsUnique(UserTelegramId telegramId)
    {
        var uniqueness = await _context.Users.AnyAsync(u =>
            u.TelegramId.TelegramId == telegramId.TelegramId
        );
        return !uniqueness;
    }
}
