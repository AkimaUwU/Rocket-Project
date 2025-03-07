using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.Utilities.Converters;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Main.Tests.ReportTaskManagement;

public class CreateReportTaskTests
{
    [Fact]
    public void Create_Report_Task_Valid_Message_And_Time()
    {
        string message = "Some text here";
        DateTime date = DateTime.Now.AddHours(2);
        Result<ReportTask> result = ReportTask.Create(message, date);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_Report_Task_InValid_Message_And_Valid_Time()
    {
        string message = "";
        DateTime date = DateTime.Now.AddHours(2);
        Result<ReportTask> result = ReportTask.Create(message, date);
        Assert.False(!result.IsSuccess);
    }

    [Fact]
    public void Create_Report_Task_InValid_Message_And_InValid_Time()
    {
        string message = "";
        DateTime date = DateTime.Now.AddHours(-22);
        Result<ReportTask> result = ReportTask.Create(message, date);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Create_Report_Task_Valid_Message_And_InValid_Time()
    {
        string message = "Some text here";
        DateTime date = DateTime.Now.AddHours(-22);
        Result<ReportTask> result = ReportTask.Create(message, date);
        Assert.False(result.IsSuccess);
    }
}
