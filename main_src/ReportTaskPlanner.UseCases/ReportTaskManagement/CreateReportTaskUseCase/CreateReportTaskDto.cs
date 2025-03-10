using ReportTaskPlanner.Main.ReportTasksManagement;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;

public sealed record CreateReportTaskDto(string Message, DateTime WhenToFire)
{
    public ReportTask Create() => ReportTask.Create(Message, WhenToFire);
}
