namespace TgBotPlannerTests.Models;

public sealed class MonthRecognizer : BasicRecognizer
{
    public MonthRecognizer()
        : base(0.7f)
    {
        AddText("январ", "1");
        AddText("январь", "1");
        AddText("января", "1");
        AddText("январе", "1");
        AddText("феврал", "2");
        AddText("февраль", "2");
        AddText("февраля", "2");
        AddText("феврале", "2");
        AddText("март", "3");
        AddText("марта", "3");
        AddText("марте", "3");
        AddText("апрел", "4");
        AddText("апрель", "4");
        AddText("апреля", "4");
        AddText("апреле", "4");
        AddText("май", "5");
        AddText("мая", "5");
        AddText("маю", "5");
        AddText("мае", "5");
        AddText("июн", "6");
        AddText("июнь", "6");
        AddText("июня", "6");
        AddText("июню", "6");
        AddText("июне", "6");
        AddText("июл", "7");
        AddText("июль", "7");
        AddText("июля", "7");
        AddText("июлю", "7");
        AddText("июле", "7");
        AddText("август", "8");
        AddText("августа", "8");
        AddText("августу", "8");
        AddText("августе", "8");
        AddText("сентябр", "9");
        AddText("сентябрь", "9");
        AddText("сентября", "9");
        AddText("сентябрю", "9");
        AddText("сентябре", "9");
        AddText("октябр", "10");
        AddText("октябрь", "10");
        AddText("октября", "10");
        AddText("октябрю", "10");
        AddText("октябре", "10");
        AddText("ноябр", "11");
        AddText("ноябрь", "11");
        AddText("ноября", "11");
        AddText("ноябрю", "11");
        AddText("ноябре", "11");
        AddText("декабр", "12");
        AddText("декабрь", "12");
        AddText("декабря", "12");
        AddText("декабрю", "12");
        AddText("декабре", "12");
    }
}
