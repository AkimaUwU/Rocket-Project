using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Main.TimeManagement;

public interface IDateTimeParser
{
    public Result<DateTime> Parse(string text);
}
