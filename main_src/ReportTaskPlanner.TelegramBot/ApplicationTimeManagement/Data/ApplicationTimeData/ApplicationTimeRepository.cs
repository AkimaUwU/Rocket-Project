using System.Data.Common;
using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Data;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;

public sealed class ApplicationTimeRepository(ApplicationTimeDbContext context)
    : IApplicationTimeRepository
{
    private readonly ApplicationTimeDbContext _context = context;
    private const string GetCountSql = "SELECT Count(*) FROM Application_Time";

    private const string InsertSql = """
                INSERT 
                INTO Application_Time (ZoneName, time_stamp, display_name, date_time)
                VALUES (@ZoneName, @time_stamp, @display_name, @date_time)
        """;

    private const string UpdateSql = """
        UPDATE Application_Time 
        SET ZoneName = @ZoneName_Old, time_stamp = @time_stamp, display_name = @display_name, date_time = @date_time 
        WHERE ZoneName = @ZoneName_New
        """;

    private const string SelectSql = "SELECT * FROM Application_Time";

    private const string DeleteSql = "DELETE FROM Application_Time WHERE ZoneName = @ZoneName";

    public async Task<Result> Save(ApplicationTime time)
    {
        var parameters = new
        {
            @ZoneName = time.ZoneName,
            @time_stamp = time.TimeStamp,
            @display_name = time.DisplayName,
            @date_time = time.DateTime,
        };
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int recordsCount = await connection.ExecuteScalarAsync<int>(GetCountSql);
            if (recordsCount >= 1)
                return new Error("Уже есть существующая конфигурация времени приложения.");
            await connection.ExecuteAsync(InsertSql, parameters);
            return await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                "Не удалось сохранить конфигурация времени приложения. Ошибка в сохранении в БД."
            );
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<Result<ApplicationTime>> Get()
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        dynamic? time = await connection.QueryFirstOrDefaultAsync(SelectSql);
        if (time == null)
            return new Error("Конфигурация времени приложения не была создана.");
        string zoneName = time.ZoneName;
        string displayName = time.display_name;
        long timeStamp = time.time_stamp;
        DateTime dateTime = DateTime.ParseExact(
            time.date_time,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture
        );
        return new ApplicationTime(zoneName, displayName, timeStamp, dateTime);
    }

    public async Task<Result> Update(ApplicationTime time)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            Result<ApplicationTime> current = await Get();
            if (current.IsFailure)
                return current.Error;
            string zoneNameOld = current.Value.ZoneName;
            var parameters = new
            {
                @ZoneName_Old = zoneNameOld,
                @ZoneName_New = time.ZoneName,
                @time_stamp = time.TimeStamp,
                @display_name = time.DisplayName,
                @date_time = time.DateTime,
            };
            int affectedCount = await connection.ExecuteAsync(UpdateSql, parameters);
            return affectedCount == 0
                ? new Error(
                    "Не удалось обновить конфигурацию времени бота. Предыдущая конфигурация не найдена."
                )
                : await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                "Не удалось обновить конфигурацию времени бота. Ошибка при обновлении записи в БД."
            );
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<Result> Delete(string zoneName)
    {
        var parameters = new { @ZoneName = zoneName };
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int affectedRows = await connection.ExecuteAsync(DeleteSql, parameters);
            if (affectedRows == 0)
                return new Error(
                    $"Не удалось удалить конфигурацию времени бота с именем зоны: {zoneName}. Не найдено."
                );
            return await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new(
                $"Не удалось удалить конфигурацию времени бота с именем зоны: {zoneName}. Ошибка при удалении записи из БД."
            );
            return await transaction.RollbackAndReturn(error);
        }
    }
}
