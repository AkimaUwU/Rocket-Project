namespace TgBotPlannerTests.Models;

public sealed class DayOfWeekRecognizer : BasicRecognizer
{
    public DayOfWeekRecognizer()
        : base(0.7f)
    {
        AddText("понедельн", "понедельник");
        AddText("понедельни", "понедельник");
        AddText("понедельник", "понедельник");
        AddText("вторник", "вторник");
        AddText("среда", "среда");
        AddText("среду", "среда");
        AddText("четверг", "четверг");
        AddText("пятниц", "пятница");
        AddText("суббот", "суббота");
        AddText("воскресень", "воскресенье");
    }
}
