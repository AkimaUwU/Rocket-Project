using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.TasksDatabase;

public sealed class TasksDatabase : ITaskDataBase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task AddTask(TasksDao task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Attach(task.Owner);
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public async Task Remove(TasksDao task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Tasks.Where(t => t.Id == task.Id).ExecuteDeleteAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public async Task Update(TasksDao task)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context
                .Tasks.Where(t => t.Id == task.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(b => b.NotifyDate, task.NotifyDate));
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }
}
