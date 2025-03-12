using System.Text.RegularExpressions;
using ReportTaskPlanner.RegexDateTimeParser.Types.DateTypes;
using ReportTaskPlanner.RegexDateTimeParser.Types.RegexMatches;
using ReportTaskPlanner.RegexDateTimeParser.Types.RegexTypes;

namespace ReportTaskPlanner.Integrationa.Tests.RegexDateTimeParserTests;

public sealed class RegexDateTimeParserTest : BaseTest
{
    [Fact]
    public void Create_Relative_Date_Type()
    {
        string input1 = "вчера в 16:35 сходить в магазин";
        string input2 = "сегодня в 16:35 сходить в магазин";
        string input3 = "завтра в 16:35 сходить в магазин";
        string input4 = "послезавтра в 16:35 сходить в магазин";

        RegexType[] regexes = RegexTypeList.Array;

        RegexMatch match1 = RegexTypeMatching.TryMatch(input1, regexes);
        RegexMatch match2 = RegexTypeMatching.TryMatch(input2, regexes);
        RegexMatch match3 = RegexTypeMatching.TryMatch(input3, regexes);
        RegexMatch match4 = RegexTypeMatching.TryMatch(input4, regexes);

        DateType date1 = match1.ConvertToDateType();
        DateType date2 = match2.ConvertToDateType();
        DateType date3 = match3.ConvertToDateType();
        DateType date4 = match4.ConvertToDateType();

        Assert.IsType<UnknownDateType>(date1);
        Assert.IsType<RelativeDateType>(date2);
        Assert.IsType<RelativeDateType>(date3);
        Assert.IsType<RelativeDateType>(date4);
    }

    [Fact]
    public void Create_Date_Of_Week_Date_Type()
    {
        string input1 = "сегодня в понедельник заказать пиццу в 16:35";
        string input2 = "сегодня во вторник заказать пиццу в 16 35";
        string input3 = "сегодня в среду заказать пиццу в 16 35";
        string input4 = "в четверг заказать пиццу в 16 35";
        string input5 = "сегодня в пятницу заказать пиццу в 16:35";
        string input6 = "сегодня в субботу заказать пиццу в 16 35";
        string input7 = "сегодня в воскресенье заказать пиццу в 16:35";

        RegexType[] regexes = RegexTypeList.Array;
        RegexMatch match1 = RegexTypeMatching.TryMatch(input1, regexes);
        RegexMatch match2 = RegexTypeMatching.TryMatch(input2, regexes);
        RegexMatch match3 = RegexTypeMatching.TryMatch(input3, regexes);
        RegexMatch match4 = RegexTypeMatching.TryMatch(input4, regexes);
        RegexMatch match5 = RegexTypeMatching.TryMatch(input5, regexes);
        RegexMatch match6 = RegexTypeMatching.TryMatch(input6, regexes);
        RegexMatch match7 = RegexTypeMatching.TryMatch(input7, regexes);

        Assert.IsType<DayOfWeekRegexMatch>(match1);
        Assert.IsType<DayOfWeekRegexMatch>(match2);
        Assert.IsType<DayOfWeekRegexMatch>(match3);
        Assert.IsType<DayOfWeekRegexMatch>(match4);
        Assert.IsType<DayOfWeekRegexMatch>(match5);
        Assert.IsType<DayOfWeekRegexMatch>(match6);
        Assert.IsType<DayOfWeekRegexMatch>(match7);

        DateType date1 = match1.ConvertToDateType();
        DateType date2 = match2.ConvertToDateType();
        DateType date3 = match3.ConvertToDateType();
        DateType date4 = match4.ConvertToDateType();
        DateType date5 = match5.ConvertToDateType();
        DateType date6 = match6.ConvertToDateType();
        DateType date7 = match7.ConvertToDateType();

        Assert.IsType<WeekedWithTimeDateType>(date1);
        Assert.IsType<WeekedWithTimeDateType>(date2);
        Assert.IsType<RelativeWithTimeDateType>(date3);
        Assert.IsType<WeekedWithTimeDateType>(date4);
        Assert.IsType<WeekedWithTimeDateType>(date5);
        Assert.IsType<WeekedWithTimeDateType>(date6);
        Assert.IsType<WeekedWithTimeDateType>(date7);
    }

    [Fact]
    public void Create_Date_Of_Month_Date_Type()
    {
        string input1 = "13 марта купить пиццу";
        string input2 = "13 марта купить пиццу в 16 35";
        string input3 = "13 марта купить пиццу в 16:35";
        string input4 = "марта купить пиццу";
        string input5 = "марта купить пиццу в 16 35";
        string input6 = "марта купить пиццу в 16:35";

        RegexType[] regexes = RegexTypeList.Array;

        RegexMatch match1 = RegexTypeMatching.TryMatch(input1, regexes);
        RegexMatch match2 = RegexTypeMatching.TryMatch(input2, regexes);
        RegexMatch match3 = RegexTypeMatching.TryMatch(input3, regexes);
        RegexMatch match4 = RegexTypeMatching.TryMatch(input4, regexes);
        RegexMatch match5 = RegexTypeMatching.TryMatch(input5, regexes);
        RegexMatch match6 = RegexTypeMatching.TryMatch(input6, regexes);

        Assert.IsType<UnknownRegexMatch>(match1);
        Assert.IsType<MonthWithTimeRegexMatch>(match2);
        Assert.IsType<MonthWithTimeRegexMatch>(match3);
        Assert.IsType<UnknownRegexMatch>(match4);
        Assert.IsType<UnknownRegexMatch>(match5);
        Assert.IsType<UnknownRegexMatch>(match6);

        DateType date1 = match1.ConvertToDateType();
        DateType date2 = match2.ConvertToDateType();
        DateType date3 = match3.ConvertToDateType();
        DateType date4 = match4.ConvertToDateType();
        DateType date5 = match5.ConvertToDateType();
        DateType date6 = match6.ConvertToDateType();

        Assert.IsType<UnknownDateType>(date1);
        Assert.IsType<MonthDateType>(date2);
        Assert.IsType<MonthDateType>(date3);
        Assert.IsType<UnknownDateType>(date4);
        Assert.IsType<UnknownDateType>(date5);
        Assert.IsType<UnknownDateType>(date6);
    }
}
