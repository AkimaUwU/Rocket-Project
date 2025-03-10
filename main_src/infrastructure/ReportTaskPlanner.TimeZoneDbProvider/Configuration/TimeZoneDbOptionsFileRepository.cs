using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.TimeZoneDbProvider.Configuration;

public sealed record TimeZoneDbOptionsFileRepository
{
    private readonly GetTimeZoneDbOptionsFromFile _get;
    private readonly SaveTimeZoneDbOptionsAsFile _save;
    private readonly UpdateTimeZoneDbOptions _update;
    public TimeZoneDbOptionsFileRepository(GetTimeZoneDbOptionsFromFile get, SaveTimeZoneDbOptionsAsFile save, UpdateTimeZoneDbOptions update) =>
        (_get, _save, _update) = (get, save, update);
    
    public Result<TimeZoneDbOptions> Get(string filePath) => _get(filePath);
    public Result Save(string filePath, string token) => _save(filePath, token);
    public Result Update(string filePath, TimeZoneDbOptions options) => _update(filePath, options);
}