namespace ReportTaskPlanner.LiteDb;

[AttributeUsage(AttributeTargets.Class)]
public class DbQueryAttribute : Attribute
{
    public Type BaseType { get; set; } = default!;
}
