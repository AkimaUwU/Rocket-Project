using LiteDB;
using ReportTaskPlanner.Main.ReportTasksManagement;

namespace ReportTaskPlanner.LiteDb.ReportTaskManagement;

public sealed record ReportTaskDao
{
    public const string Collection = "Report_Tasks";
    public Guid Id { get; }
    public long UnixDateCreated { get; }
    public long UnixDateWhenToFire { get; }
    public long UnixRemained { get; }
    public string Message { get; }

    public ReportTaskDao(ReportTask domainModel)
    {
        Id = domainModel.Id;
        UnixDateCreated = domainModel.WhenCreated.Seconds;
        UnixDateWhenToFire = domainModel.WhenToFire.Seconds;
        UnixRemained = domainModel.RemainedUntilFire.Seconds;
        Message = domainModel.Message;
    }

    [DbObjectMapping]
    public static void RegisterMapping()
    {
        BsonMapper
            .Global.Entity<ReportTaskDao>()
            .Id(x => x.Id, false)
            .Field(x => x.Message, "task_message")
            .Field(x => x.UnixDateCreated, "task_created")
            .Field(x => x.UnixDateWhenToFire, "task_time_to_fire")
            .Field(x => x.UnixRemained, "task_time_remain");
    }
}
