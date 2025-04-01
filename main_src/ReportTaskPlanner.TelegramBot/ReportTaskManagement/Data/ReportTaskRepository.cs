using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Data;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public sealed class ReportTaskRepository(ReportTaskDbContext context) : IReportTaskRepository
{
    private readonly ReportTaskDbContext _context = context;

    private const string InsertSql = """
        INSERT
        INTO Report_Tasks
        (Id, task_text, task_schedule_created_unix, task_schedule_notify_time, task_schedule_is_periodic)
        VALUES
        (@Id, @task_text, @task_schedule_created_unix, @task_schedule_notify_time, @task_schedule_is_periodic)
        """;

    private const string QueryPendingSql = """
        SELECT * FROM Report_Tasks
        Where task_schedule_notify_time <= @time
        """;

    private const string DeletePendingSql = """
        DELETE FROM Report_Tasks
        Where Id in @ids
        """;

    private const string UpdateTaskTimeByIdSql = """
            UPDATE Report_Tasks
            SET
                task_schedule_created_unix = CASE Id
                    {0}
                    END,
                task_schedule_notify_time = CASE Id
                    {1}
                    END,
                task_schedule_is_periodic = CASE Id
                    {2}
                    END
            WHERE Id IN ({3});
        """;

    public async Task<Result> Save(ReportTask task)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = connection.BeginTransaction();
        var parameters = new
        {
            @Id = task.Id,
            @task_text = task.Text,
            @task_schedule_created_unix = task.Schedule.TimeCreated,
            @task_schedule_notify_time = task.Schedule.TimeToNotify,
            @task_schedule_is_periodic = task.Schedule.IsPeriodic,
        };
        try
        {
            await connection.ExecuteAsync(InsertSql, parameters);
            return await transaction.CommitAndReturn(Result.Success());
        }
        catch
        {
            Error error = new("Не удалось создать задачу. Ошибка при записи в БД.");
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<ReportTask[]> GetPendingTasks(ApplicationTime time)
    {
        await using SqliteConnection connection = await _context.CreateConnection();
        dynamic[] data = (
            await connection.QueryAsync(QueryPendingSql, new { @time = time.TimeStamp })
        ).ToArray();
        ReportTask[] mapped = new ReportTask[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            long itemId = data[i].Id;
            string text = data[i].task_text;
            long timeCreated = data[i].task_schedule_created_unix;
            long timeNotify = data[i].task_schedule_notify_time;
            long isPeriodicNumber = data[i].task_schedule_is_periodic;
            bool isPeriodic = isPeriodicNumber == 1;
            ReportTaskSchedule schedule = new(timeCreated, timeNotify, isPeriodic);
            mapped[i] = new ReportTask(itemId, text, schedule);
        }

        return mapped;
    }

    public async Task<Result<int>> RemoveMany(IEnumerable<ReportTask> tasks)
    {
        ReportTask[] array = tasks.ToArray();
        if (array.Length == 0)
            return 0;
        long[] identifiers = [.. array.Select(t => t.Id)];
        var parameters = new { @ids = identifiers };
        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int affectedCount = await connection.ExecuteAsync(DeletePendingSql, parameters);
            return await transaction.CommitAndReturn(affectedCount);
        }
        catch
        {
            Error error = new("Не удалось удалить задачи. Ошибка при удалении записей из БД.");
            return await transaction.RollbackAndReturn(error);
        }
    }

    public async Task<Result<int>> UpdateMany(IEnumerable<ReportTask> tasks)
    {
        ReportTask[] array = tasks.ToArray();
        if (array.Length == 0)
            return 0;

        StringBuilder whenClauses = new();
        DynamicParameters parameters = new();
        List<long> taskIds = [];

        foreach (var task in array)
        {
            string taskIdParam = $"@Id_{task.Id}";
            string createdUnixParam = $"@task_schedule_created_unix_{task.Id}";
            string notifyTimeParam = $"@task_schedule_notify_time_{task.Id}";
            string isPeriodicParam = $"@task_schedule_is_periodic_{task.Id}";

            whenClauses.AppendLine($"WHEN {taskIdParam} THEN {createdUnixParam}");
            whenClauses.AppendLine($"WHEN {taskIdParam} THEN {notifyTimeParam}");
            whenClauses.AppendLine($"WHEN {taskIdParam} THEN {isPeriodicParam}");

            taskIds.Add(task.Id);

            parameters.Add(taskIdParam, task.Id);
            parameters.Add(createdUnixParam, task.Schedule.TimeCreated);
            parameters.Add(notifyTimeParam, task.Schedule.TimeToNotify);
            parameters.Add(isPeriodicParam, task.Schedule.IsPeriodic);
        }

        string whenCreatedUnixPart = whenClauses.ToString();
        string whenNotifyTimePart = whenCreatedUnixPart.Replace("created_unix", "notify_time");
        string whenIsPeriodicPart = whenCreatedUnixPart.Replace("created_unix", "is_periodic");
        string taskIdsPart = string.Join(", ", taskIds);

        string finalSql = string.Format(
            UpdateTaskTimeByIdSql,
            whenCreatedUnixPart,
            whenNotifyTimePart,
            whenIsPeriodicPart,
            taskIdsPart
        );

        await using SqliteConnection connection = await _context.CreateConnection();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();
        try
        {
            int affectedRows = await connection.ExecuteAsync(finalSql, parameters);
            return await transaction.CommitAndReturn(affectedRows);
        }
        catch
        {
            Error error = new("Ошибка при обновлении записей задач.");
            return await transaction.RollbackAndReturn(error);
        }
    }
}
