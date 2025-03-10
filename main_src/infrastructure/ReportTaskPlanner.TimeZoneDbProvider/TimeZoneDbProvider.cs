// using ReportTaskPlanner.Main.TimeManagement;
// using TimeProvider = ReportTaskPlanner.Main.TimeManagement.TimeProvider;
//
// namespace ReportTaskPlanner.TimeZoneDbProvider;
//
// public sealed record TimeZoneDbProvider : TimeProvider
// {
//     private readonly TimeZoneDbOptions _options;
//     public TimeZoneDbProvider(TimeZoneDbOptions options) => _options = options;
//     public async override Task<IEnumerable<PlannerTime>> ListTimes()
//     {
//         using HttpClient client = new HttpClient();
//         string url = $"https://api.timezonedb.com/v2.1/list-time-zone&key={_options.Token}&format=json&country=RU";
//         HttpResponseMessage response = await client.GetAsync(url);
//         string responseJson = await response.Content.ReadAsStringAsync();
//         TimeZoneDbOptionsJsonReader reader = new TimeZoneDbOptionsJsonReader(responseJson);
//         return reader.ConvertToPlannerTimeEnumerable();
//     }
//
//     public async override Task<PlannerTime> GetUpdatedTime(PlannerTime plannerTime)
//     {
//         using HttpClient client = new HttpClient();
//         string url =
//             $"https://api.timezonedb.com/v2.1/list-time-zone&key={_options.Token}&format=json&country=RU&zone={plannerTime.ZoneName}";
//         HttpResponseMessage response = await client.GetAsync(url);
//         string responseJson = await response.Content.ReadAsStringAsync();
//         TimeZoneDbOptionsJsonReader reader = new TimeZoneDbOptionsJsonReader(responseJson);
//         PlannerTime? updated = reader.ConvertToPlannerTimeEnumerable()
//             .FirstOrDefault(zone => zone.ZoneName == plannerTime.ZoneName); 
//         if (updated == null)
//             
//     }
// }