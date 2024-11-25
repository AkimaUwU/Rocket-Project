using Microsoft.EntityFrameworkCore;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.DataAccess.DataBase;

namespace RocketPlaner.DataAccess.DatabaseImplementations.TasksDestinationsDataBase;

public sealed class TaskDestinationsDatabase : ITaskDestinationDatabase
{
    private readonly DataBaseContext _context = new DataBaseContext();

    public async Task AddDestination(DestinationsDao destination)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Destinations.Entry(destination).State = EntityState.Added;
            await _context.Destinations.AddAsync(destination);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }

    public async Task RemoveDestination(DestinationsDao destination)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context
                .Destinations.Where(dest => dest.Id == destination.Id)
                .ExecuteDeleteAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}
