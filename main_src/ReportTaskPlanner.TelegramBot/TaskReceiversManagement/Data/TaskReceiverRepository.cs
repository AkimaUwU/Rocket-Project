using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using ReportTaskPlanner.TelegramBot.Shared.Data;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public sealed class TaskReceiverRepository(TaskReceiverDbContext context) : ITaskReceiverRepository
{
    private readonly TaskReceiverDbContext _context = context;

    private const string GetDuplicateSql = "SELECT Id FROM Task_Receivers WHERE Id = @receiver_id";

    private const string InsertSql = """
        INSERT
        INTO Task_Receivers (Id, is_enabled)
        VALUES (@receiver_id, @is_enabled)
        """;

    private const string SelectAllSql = """
        SELECT Id, is_enabled
        FROM Task_Receivers
        """;

    private const string DeleteSql = """
        DELETE
        FROM Task_Receivers
        WHERE
        Id = @receiver_id
        """;

    public async Task<Result> Save(TaskReceiver receiver)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            TaskReceiver? existing = await connection.QueryFirstOrDefaultAsync<TaskReceiver>(
                GetDuplicateSql,
                new { @receiver_id = receiver.Id }
            );
            if (existing != null)
                return new Error($"Чат с ID: {existing.Id} уже добавлен.");
            await connection.ExecuteAsync(
                InsertSql,
                new { @receiver_id = receiver.Id, @is_enabled = receiver.IsEnabled }
            );
            return await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                $"Не удалось добавить чат с ID: {receiver.Id}. Ошибка при сохранении в БД."
            );
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<TaskReceiver[]> GetAll()
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        return (await connection.QueryAsync<TaskReceiver>(SelectAllSql)).ToArray();
    }

    public async Task<Result> Remove(long id)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int rowsAffected = await connection.ExecuteAsync(DeleteSql, new { @receiver_id = id });
            return rowsAffected == 1
                ? await transaction.CommitAndReturn(Result.Success())
                : new Error($"Не удалось удалить чат с ID: {id}. Не был найден.");
        }
        catch
        {
            Error error = new($"Не удалось удалить чат с ID: {id}. Ошибка при удалении из БД.");
            return await transaction.RollbackAndReturn(error);
        }
    }
}
