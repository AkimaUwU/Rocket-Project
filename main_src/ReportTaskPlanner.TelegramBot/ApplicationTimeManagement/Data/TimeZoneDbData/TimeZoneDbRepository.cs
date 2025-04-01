using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using ReportTaskPlanner.TelegramBot.Shared.Data;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;

public sealed class TimeZoneDbRepository(TimeZoneDbContext context) : ITimeZoneDbRepository
{
    private readonly TimeZoneDbContext _context = context;

    private const string CountQuery = """
        SELECT COUNT(*) FROM Time_Zone_Db_Options
        """;

    private const string DeleteQuery = """
        DELETE
        FROM Time_Zone_Db_Options
        WHERE Token = @token
        """;

    private const string InsertQuery = """
        INSERT
        INTO Time_Zone_Db_Options (Token)
        VALUES (@token)
        """;

    private const string SelectQuery = """
        SELECT
        Token
        FROM
        Time_Zone_Db_Options
        """;

    private const string UpdateQuery = """
        UPDATE Time_Zone_Db_Options
        SET Token = @token_new
        Where Token = @token_old
        """;

    public async Task<Result> Save(TimeZoneDbOptions options)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int count = await connection.ExecuteScalarAsync<int>(CountQuery);
            if (count >= 1)
            {
                return new Error(
                    "Токен Time Zone Db уже присутствует. Если нужно изменить токен - вызовите функцию обновления."
                );
            }

            await connection.ExecuteAsync(InsertQuery, new { @token = options.Token });
            return await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                "Не удалось сохранить конфигурацию Time Zone Db. Ошибка при сохранении в БД"
            );
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<Result<TimeZoneDbOptions>> Get()
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        TimeZoneDbOptions? options = await connection.QueryFirstOrDefaultAsync<TimeZoneDbOptions>(
            SelectQuery
        );
        return options == null ? new Error("Time Zone Db конфигурации нет в приложении.") : options;
    }

    public async Task<Result> Update(TimeZoneDbOptions options)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            TimeZoneDbOptions? current = (
                await connection.QueryAsync<TimeZoneDbOptions>(SelectQuery)
            ).FirstOrDefault();
            if (current == null)
                return new Error(
                    "Не удалось обновить конфигурацию Time Zone Db. Не найдена существующая конфигурация."
                );
            string currentToken = current.Token;
            var parameters = new { @token_new = options.Token, @token_old = currentToken };
            int rowsAffected = await connection.ExecuteAsync(UpdateQuery, parameters);
            return rowsAffected == 0
                ? new Error(
                    "Не удалось обновить конфигурацию Time Zone Db. Не найдена существующая конфигурация."
                )
                : await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                "Не удалось обновить конфигурацию Time Zone Db. Произошла ошибка при обновлении записи в БД."
            );
            return error;
        }
    }

    public async Task<Result> Delete(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return new Error("Токен для удаления существующего токена Time Zone Db не указан.");
        var parameters = new { @token = token };
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int rowsAffected = await connection.ExecuteAsync(DeleteQuery, parameters);
            await transaction.CommitAsync();
            return rowsAffected == 0
                ? new Error(
                    $"Не удалось удалить Time Zone Db токен со значением {token}. Не найден."
                )
                : Result.Success();
        }
        catch
        {
            Error error = new(
                $"Не удалось удалить токен Time Zone Db со значением {token}. Ошибка при удалении записи из БД."
            );
            return await transaction.RollbackAndReturn(error);
        }
    }
}
