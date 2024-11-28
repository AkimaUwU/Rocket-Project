using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.TasksDatabase;

public sealed class TasksDatabase : ITaskDataBase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task AddTask(RocketTask task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Attach(task.Owner);
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task Remove(RocketTask task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task Update(RocketTask task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}
