using ReportTaskPlanner.Utilities.Common;
using ReportTaskPlanner.Utilities.Converters;
using ReportTaskPlanner.Utilities.Converters.DateConverters.Unix;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Main.ReportTasksManagement;

public sealed record ReportTask
{
    public Guid Id { get; private init; } = Guid.Empty;
    public string Message { get; private init; } = String.Empty;
    public UnixDateSeconds WhenCreated { get; private init; }
    public UnixDateSeconds WhenToFire { get; private init; }

    public UnixDateSeconds RemainedUntilFire { get; private init; }

    private ReportTask()
    {
        Id = Guid.Empty;
        Message = string.Empty;
        WhenCreated = UnixDateSeconds.Zero;
        WhenToFire = UnixDateSeconds.Zero;
    }

    public sealed record Factory(string message, DateTime whenToFire)
    {
        public TypedResult<ReportTask> Create()
        {
            UnixDateConverter toUnixNow = new FromDateTimeConverter(DateTime.Now);
            UnixDateConverter toUnixWhenToFire = new FromDateTimeConverter(whenToFire);
            ConvertProcess process = new ConvertProcess();

            UnixDateSeconds unixCurrent = new(process.Convert(toUnixNow));
            if (unixCurrent.Seconds < 0)
                return new FailureTypedResult<ReportTask>("Время создания задачи некорректно.");

            UnixDateSeconds unixToFire = new(process.Convert(toUnixWhenToFire));
            UnixDateSeconds remained = unixToFire.GetDifferenceFrom(unixCurrent);
            if (unixToFire.Seconds < 0 || remained.Seconds < 0)
                return new FailureTypedResult<ReportTask>("Время вызова задачи некорректно.");

            ReportTask result = new ReportTask()
            {
                Id = Guid.NewGuid(),
                Message = message,
                WhenCreated = unixCurrent,
                WhenToFire = unixToFire,
                RemainedUntilFire = remained,
            };
            return new SuccessTypedResult<ReportTask>(result);
        }
    }
}
