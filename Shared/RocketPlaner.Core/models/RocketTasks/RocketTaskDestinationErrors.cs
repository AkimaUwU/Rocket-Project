using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks;

public static class RocketTaskDestinationErrors
{
    public static readonly Error DestinationNotFound = new Error(
        "Место отправки задачи не найдено"
    );
}
