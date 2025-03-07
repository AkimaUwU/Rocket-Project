using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.CreateReportTaskUseCase;

public abstract record SaveReportTaskInPersistance
{
    public abstract Task<Result> Save(ReportTask reportTask, CancellationToken ct = default);
}
