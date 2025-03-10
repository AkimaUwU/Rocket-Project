using System.Text.Json;
using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.TimeZoneDbProvider.Configuration;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.TimeZoneDbProvider.TimeZoneDbProviderIntegration;

public sealed record TimeZoneDbRequester(
    ListPlannerTimesFromTimeZoneDb ReadJson,
    GetTimeZoneDbResponseJsonAsync RequestJson,
    SaveGlobalPlannerTime SavePlannerTime,
    UpdatePlannerTimeFromRequest UpdatePlannerTime,
    GetPlannerTimeFromJsonFile GetFromJsonFile
)
{
    public static class Actions
    {
        public static GetTimeZoneDbResponseJsonAsync GetTimeZoneDbResponseJson = async (options) =>
        {
            using HttpClient client = new HttpClient();
            string url =
                $"https://api.timezonedb.com/v2.1/list-time-zone&key={options.Token}&format=json&country=RU";
            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                string responseJson = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseJson))
                    return new Error(
                        $"Не удалось получить json ответ от Time Zone Db. Статус код: {response.StatusCode.ToString()}"
                    );
                return new TimeZoneDbResponseJson(responseJson);
            }
            catch
            {
                return new Error(
                    "Произошла внутренняя ошибка приложения при отправке запроса в Time Zone Db"
                );
            }
        };

        public static ListPlannerTimesFromTimeZoneDb ListPlannerTimesFromTimeZoneDbProvider = (
            json,
            read
        ) =>
        {
            Result<IEnumerable<PlannerTime>> result = read(json);
            ListPlannerTimes operation = () => result;
            return operation;
        };

        public static SaveGlobalPlannerTime SaveGlobalPlannerTime = (plannerTime, filePath) =>
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return new Error("Не указан путь сохранения глобального времени приложения");

            if (File.Exists(filePath))
                File.Delete(filePath);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            string serialized = JsonSerializer.Serialize(plannerTime, options);
            File.WriteAllText(filePath, serialized);
            return Result.Success();
        };

        public static GetPlannerTimeFromJsonFile ReadFromJsonFile = (filePath) =>
        {
            if (!File.Exists(filePath))
                return new Error("Файла конфигурации времени бота не существует");

            string content = File.ReadAllText(filePath);
            using JsonDocument document = JsonDocument.Parse(content);
            bool hasZoneName = document.RootElement.TryGetProperty(
                "ZoneName",
                out JsonElement zoneName
            );
            bool hasDisplayName = document.RootElement.TryGetProperty(
                "DisplayName",
                out JsonElement displayName
            );
            bool hasTimeStamp = document.RootElement.TryGetProperty(
                "TimeStamp",
                out JsonElement timeStamp
            );
            bool hasDateTime = document.RootElement.TryGetProperty(
                "DateTime",
                out JsonElement dateTime
            );

            if (!hasZoneName || !hasDisplayName || !hasTimeStamp || !hasDateTime)
                return new Error("Некорректный файл конфигурации времени бота");

            string? zoneNameValue = zoneName.GetString();
            if (string.IsNullOrWhiteSpace(zoneNameValue))
                return new Error("Некорректный файл конфигурации времени бота");

            string? displayNameValue = displayName.GetString();
            if (string.IsNullOrWhiteSpace(displayNameValue))
                return new Error("Некорректный файл конфигурации времени бота");

            return new PlannerTime(zoneNameValue, displayNameValue, timeStamp.GetUInt64());
        };

        public static UpdatePlannerTimeFromRequest UpdatePlannerTimeFromRequest = async (
            plannerTime,
            read,
            options
        ) =>
        {
            string url =
                $"https://api.timezonedb.com/v2.1/list-time-zone&key={options.Token}&format=json&country=RU&zone={plannerTime.ZoneName}";
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            string responseJson = await response.Content.ReadAsStringAsync();
            Result<IEnumerable<PlannerTime>> result = read(new(responseJson));
            if (result.IsSuccess == false)
                return result.Error;
            PlannerTime time = result.Value.First();
            return time;
        };
    }
}

public delegate Task<Result<PlannerTime>> UpdatePlannerTimeFromRequest(
    PlannerTime plannerTime,
    ReadTimeZoneDbResponseJson read,
    TimeZoneDbOptions options
);
public delegate Result<PlannerTime> GetPlannerTimeFromJsonFile(string filePath);
public delegate Result SaveGlobalPlannerTime(PlannerTime time, string filePath);
public delegate Task<Result<TimeZoneDbResponseJson>> GetTimeZoneDbResponseJsonAsync(
    TimeZoneDbOptions options
);
public delegate ListPlannerTimes ListPlannerTimesFromTimeZoneDb(
    TimeZoneDbResponseJson json,
    ReadTimeZoneDbResponseJson read
);
public delegate ListPlannerTimes ListPlannerTimesFromTimeZoneDbProvider(
    TimeZoneDbResponseJson json
);
