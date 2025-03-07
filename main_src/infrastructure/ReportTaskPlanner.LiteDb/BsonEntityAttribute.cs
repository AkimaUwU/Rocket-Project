using LiteDB;

namespace ReportTaskPlanner.LiteDb;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class BsonEntityAttribute : Attribute
{
    public BsonMappingConfig Config { get; set; } = default!;
}

public sealed record BsonMappingConfig(Action Action);
