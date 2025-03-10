using ReportTaskPlanner.DependencyInjection;
using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.UseCases.Abstractions.Handlers;
using ReportTaskPlanner.UseCases.ReportTaskManagement.CreateReportTaskUseCase;
using ReportTaskPlanner.UseCases.ReportTaskManagement.DeleteReportTaskUseCase;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Integrationa.Tests.ReporTaskManagement;

public class CreateReportTaskTest : BaseTest
{
    [Fact]
    public async Task Create_And_Save_Report_Task()
    {
        RequestDispatcher dispatcher = GetService<RequestDispatcher>();
        string message = "Сделать что-то";
        DateTime date = DateTime.Now.AddHours(2);

        CreateReportTaskDto dto = new CreateReportTaskDto(message, date);
        CreateReportTaskRequest createRequest = new CreateReportTaskRequest(dto);
        Result<ReportTask> createResult = await dispatcher.Dispatch<
            CreateReportTaskRequest,
            ReportTask
        >(createRequest);
        Assert.True(createResult.IsSuccess);

        DeleteReportTaskRequest deleteRequest = new DeleteReportTaskRequest(
            createResult.Value.Id.ToString()
        );
        Result deleteResult = await dispatcher.Dispatch(deleteRequest);
        Assert.True(deleteResult.IsSuccess);
    }
}
