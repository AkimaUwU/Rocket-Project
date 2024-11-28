using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks.Errors;

public static class RocketTaskErrors
{
    public static readonly Error RocketTaskIsNull = new Error("Задача была неопределена");

    public static readonly Error UserOwnsTaskAlready = new Error(
        "Пользователь уже создал задачу с таким заголовком"
    );

    public static readonly Error UserDoesntOwnsTask = new Error(
        "Пользователь не является обладателем этой задачи"
    );

    public static readonly Error TaskWasNotFoundInUserTasksList = new Error(
        "Задача не была найдена в списке задач пользователя"
    );

    public static readonly Error CannotHaveEmptyMessage = new Error(
        "Задача не может создаваться с пустым сообщением"
    );

    public static readonly Error CannotHaveEmptyTitle = new Error(
        "Задача не может создаваться без заголовка"
    );

    public static readonly Error MoreThanMaxSize = new Error("Задача превышает длину 128 символов");

    public static readonly Error CannotCreateWithEmptyDate = new Error(
        "Задачу нельзя создать без указания даты"
    );

    public static readonly Error CannotCreateWithPastTime = new Error(
        "Задачу нельзя создать на прошлое время"
    );

    public static readonly Error CannotCreateWithEmptyType = new Error(
        "Нельзя создать задачу без указания типа"
    );

    public static readonly Error RocketTaskTypeIsNotAllowed = new Error("Тип задачи недопустим");

    public static readonly Error OwnsDestinationAlready = new Error(
        "Уже имеет такое место отправки"
    );

    public static readonly Error DoesntOwnsDestinationChat = new Error(
        "Не имеет такого места отправки"
    );
}
