using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

public sealed class UsersDatabase : IUsersDataBase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task AddUser(UsersDao user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var task in user.Tasks)
            {
                _context.Tasks.Attach(task);
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public async Task RemoveUser(UsersDao user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Users.Where(x => x.Id == user.Id).ExecuteDeleteAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public async Task<UsersDao?> GetUser(long? telegramId)
    {
        return await _context
            .Users.Include(x => x.Tasks)
            .ThenInclude(d => d.Destinations)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    public async Task<bool> EnsureTelegramIdIsUnique(long? telegramId)
    {
        bool result = await _context.Users.AnyAsync(x => x.TelegramId == telegramId);
        return !result;
    }
}
