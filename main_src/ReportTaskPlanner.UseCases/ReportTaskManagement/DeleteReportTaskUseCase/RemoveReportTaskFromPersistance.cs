using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.DeleteReportTaskUseCase;

public abstract record RemoveReportTaskFromPersistance
{
    public abstract Task<Result> Remove(DeleteReportTaskRequest request);
}
