using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.TasksDestinationsDataBase;

public sealed class TaskDestinationsDatabase : ITaskDestinationDatabase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task RemoveDestination(RocketTaskDestination destination)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Remove(destination);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task AddDestination(RocketTaskDestination destination)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Destinations.Entry(destination).State = EntityState.Added;
            await _context.AddAsync(destination);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}
