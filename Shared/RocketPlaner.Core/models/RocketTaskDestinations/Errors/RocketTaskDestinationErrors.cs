using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTaskDestinations.Errors;

public static class RocketTaskDestinationErrors
{
    public static readonly Error DestinationNotFound = new Error(
        "Место отправки задачи не найдено"
    );

    public static readonly Error CannotCreateWithoutChatId = new Error(
        "Нельзя создать без указания ИД чата отправки"
    );

    public static readonly Error CannotCreateWithInvalidChatId = new Error(
        "Нельзя создать с таким ИД чата"
    );
}
