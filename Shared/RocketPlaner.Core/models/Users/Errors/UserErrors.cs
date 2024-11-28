using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users.Errors;

public static class UserErrors
{
    public static readonly Error CannotCreateWithoutUserTelegramId = new Error(
        "Нельзя создать пользователя без ИД телеграмма"
    );

    public static readonly Error BadTelegramIdError = new("Некорректный ID телеграмма");

    public static readonly Error TelegramIdIsNotUnuqie = new Error(
        "Пользователь с таким ID телеграмма уже имеется"
    );

    public static readonly Error UserNotFound = new Error("Пользователь не найден");
}
