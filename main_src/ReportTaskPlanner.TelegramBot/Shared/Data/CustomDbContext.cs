using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ReportTaskPlanner.TelegramBot.Shared.Data;

public class CustomDbContext : DbContext
{
    public async Task<IDbContextTransaction> StartTransaction() =>
        await Database.BeginTransactionAsync();

    public async Task<SqliteConnection> CreateConnection()
    {
        SqliteConnection connection = new(Database.GetDbConnection().ConnectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<T> CommitAndReturn<T>(IDbContextTransaction transaction, T returnValue)
    {
        await transaction.CommitAsync();
        return returnValue;
    }
}

public static class SqliteTransactionExtensions
{
    public static async Task<T> CommitAndReturn<T>(
        this SqliteTransaction transaction,
        T returnValue
    )
    {
        await transaction.CommitAsync();
        return returnValue;
    }

    public static async Task<T> RollbackAndReturn<T>(
        this SqliteTransaction transaction,
        T returnValue
    )
    {
        await transaction.RollbackAsync();
        return returnValue;
    }

    public static async Task<T> CommitAndReturn<T>(this DbTransaction transaction, T returnValue)
    {
        await transaction.CommitAsync();
        return returnValue;
    }

    public static async Task<T> RollbackAndReturn<T>(this DbTransaction transaction, T returnValue)
    {
        await transaction.RollbackAsync();
        return returnValue;
    }
}
