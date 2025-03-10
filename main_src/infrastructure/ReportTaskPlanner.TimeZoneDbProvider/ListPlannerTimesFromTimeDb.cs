// using ReportTaskPlanner.Main.TimeManagement;
// using ReportTaskPlanner.Utilities.ResultPattern;
//
// namespace ReportTaskPlanner.TimeZoneDbProvider;
//
// public sealed record ListPlannerTimesFromTimeDb
// {
//     private readonly TimeZoneDbOptions _options;
//     
//     public ListPlannerTimesFromTimeDb(TimeZoneDbOptions options) => _options = options;
//
//     private async Task<Result<IEnumerable<PlannerTime>>> GetPlannerTimeFromRequest()
//     {
//         if (string.IsNullOrWhiteSpace(_options.Token))
//             return new Error("Нет токена от Time Zone Db провайдера");
//         using HttpClient client = new HttpClient();
//         string url = $"https://api.timezonedb.com/v2.1/list-time-zone&key={_options.Token}&format=json&country=RU";
//         HttpResponseMessage response = await client.GetAsync(url);
//         TimeZoneDbOptionsJsonReader reader = new TimeZoneDbOptionsJsonReader(await response.Content.ReadAsStringAsync());
//         IEnumerable<PlannerTime> times = reader.ConvertToPlannerTimeEnumerable();
//         return Result<IEnumerable<PlannerTime>>.Success(times);
//     }
//
//     public ListPlannerTimesAsync ListPlannerTimes = async () => await GetPlannerTimeFromRequest();
// }