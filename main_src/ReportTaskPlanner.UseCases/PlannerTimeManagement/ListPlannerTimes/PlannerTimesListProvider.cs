using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.PlannerTimeManagement.ListPlannerTimes;

public abstract record PlannerTimesListProvider
{
    public abstract Task<Result<IEnumerable<PlannerTime>>> ListPlannerTimes();
}