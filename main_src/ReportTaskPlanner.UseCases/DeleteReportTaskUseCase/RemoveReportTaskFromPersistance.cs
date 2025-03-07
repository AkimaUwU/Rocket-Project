using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.DeleteReportTaskUseCase;

public abstract record RemoveReportTaskFromPersistance
{
    public abstract Task<Result> Remove(DeleteReportTaskRequest request);
}
