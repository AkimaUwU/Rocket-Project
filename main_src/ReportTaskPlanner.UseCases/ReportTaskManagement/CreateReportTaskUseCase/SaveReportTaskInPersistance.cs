using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;

public abstract record SaveReportTaskInPersistance
{
    public abstract Task<Result> Save(ReportTask reportTask, CancellationToken ct = default);
}
