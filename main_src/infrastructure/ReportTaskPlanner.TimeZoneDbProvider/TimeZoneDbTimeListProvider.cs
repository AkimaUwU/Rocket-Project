// using ReportTaskPlanner.Main.TimeManagement;
// using ReportTaskPlanner.UseCases.PlannerTimeManagement.ListPlannerTimes;
// using ReportTaskPlanner.Utilities.ResultPattern;
// using Serilog;
//
// namespace ReportTaskPlanner.TimeZoneDbProvider;
//
// public sealed record TimeZoneDbTimeListProvider : PlannerTimesListProvider
// {
//     private readonly TimeZoneDbOptions _options;
//     private readonly ILogger _logger;
//
//     public TimeZoneDbTimeListProvider(TimeZoneDbOptions options, ILogger logger) =>
//         (_options, _logger) = (options, logger);
//     
//     public async override Task<Result<IEnumerable<PlannerTime>>> ListPlannerTimes()
//     {
//         
//     }
// }