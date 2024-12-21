namespace RocketPlanner.TimeClassifier.Data;

public sealed class InMemoryTrainingData
{
    public IEnumerable<InputTime> GenerateTrainingData()
    {
        yield return new InputTime() { Text = "час", Label = "Час" };
        yield return new InputTime() { Text = "минута", Label = "Минута" };
        yield return new InputTime() { Text = "секунда", Label = "Секунда" };
        yield return new InputTime() { Text = "часов", Label = "Час" };
        yield return new InputTime() { Text = "минут", Label = "Минута" };
        yield return new InputTime() { Text = "секунды", Label = "Секунда" };
        yield return new InputTime() { Text = "через час", Label = "Час" };
        yield return new InputTime() { Text = "через минут", Label = "Минута" };
        yield return new InputTime() { Text = "через секунды", Label = "Секунда" };
        yield return new InputTime() { Text = "14:30", Label = "Время" };
        yield return new InputTime() { Text = "8 вечера", Label = "Время" };
        yield return new InputTime() { Text = "понедельник", Label = "День Недели" };
        yield return new InputTime() { Text = "вторник", Label = "День Недели" };
        yield return new InputTime() { Text = "среда", Label = "День Недели" };
        yield return new InputTime() { Text = "четверг", Label = "День Недели" };
        yield return new InputTime() { Text = "пятница", Label = "День Недели" };
        yield return new InputTime() { Text = "суббота", Label = "День Недели" };
        yield return new InputTime() { Text = "воскресенье", Label = "День Недели" };
        yield return new InputTime() { Text = "завтра", Label = "День" };
        yield return new InputTime() { Text = "послезавтра", Label = "День" };
        yield return new InputTime() { Text = "сегодня", Label = "День" };
        yield return new InputTime() { Text = "вчера", Label = "День" };
        yield return new InputTime() { Text = "10 утра", Label = "Время" };
        yield return new InputTime() { Text = "21:00", Label = "Время" };
        yield return new InputTime() { Text = "через 5 минут", Label = "Минута" };
        yield return new InputTime() { Text = "через 10 часов", Label = "Час" };
        yield return new InputTime() { Text = "через 30 секунд", Label = "Секунда" };
        yield return new InputTime() { Text = "11 утра", Label = "Время" };
        yield return new InputTime() { Text = "3 дня", Label = "Время" };
        yield return new InputTime() { Text = "9 вечера", Label = "Время" };
        yield return new InputTime() { Text = "6 часов", Label = "Час" };
        yield return new InputTime() { Text = "18:15", Label = "Время" };
        yield return new InputTime() { Text = "часа", Label = "Час" };
        yield return new InputTime() { Text = "минут", Label = "Минута" };
        yield return new InputTime() { Text = "секунд", Label = "Секунда" };
        yield return new InputTime() { Text = "через 20 минут", Label = "Минута" };
        yield return new InputTime() { Text = "10:00", Label = "Время" };
        yield return new InputTime()
        {
            Text =
                "Но сегодня предлагаем пойти по другому сценарию — вспомнить самое важное и закрыть уже наконец все свои гештальты.",
            Label = "NoTime",
        };
        yield return new InputTime() { Text = "Получите бесплатный подарок!", Label = "NoTime" };
        yield return new InputTime() { Text = "Ваша заявка одобрена!", Label = "NoTime" };
        yield return new InputTime() { Text = "У вас выигрыш!", Label = "NoTime" };
        yield return new InputTime() { Text = "Не упустите шанс!", Label = "NoTime" };
        yield return new InputTime() { Text = "Ваш кредит одобрен!", Label = "NoTime" };
        yield return new InputTime() { Text = "Только сегодня скидка 50%!", Label = "NoTime" };
        yield return new InputTime() { Text = "Срочно! Акция!", Label = "NoTime" };
        yield return new InputTime() { Text = "Просто текст без времени.", Label = "NoTime" };
        yield return new InputTime()
        {
            Text = "Ещё один пример без временных указаний.",
            Label = "NoTime",
        };
        yield return new InputTime()
        {
            Text = "Текст, не содержащий информации о времени.",
            Label = "NoTime",
        };
        yield return new InputTime() { Text = "Закажите сейчас!", Label = "NoTime" };
        yield return new InputTime() { Text = "Невероятная цена!", Label = "NoTime" };
        yield return new InputTime() { Text = "Ограниченное предложение!", Label = "NoTime" };
        yield return new InputTime() { Text = "Присоединяйтесь к нам!", Label = "NoTime" };
        yield return new InputTime() { Text = "Лучшее предложение года!", Label = "NoTime" };
        yield return new InputTime() { Text = "я", Label = "NoTime" };
        yield return new InputTime() { Text = "пошёл", Label = "NoTime" };
        yield return new InputTime() { Text = "гулять", Label = "NoTime" };
        yield return new InputTime() { Text = "товар", Label = "NoTime" };
        yield return new InputTime() { Text = "предмет", Label = "NoTime" };
    }

    public IEnumerable<InputTime> GenerateTestData()
    {
        yield return new InputTime() { Text = "2 часа", Label = "Час" };
        yield return new InputTime() { Text = "15 минут", Label = "Минута" };
        yield return new InputTime() { Text = "30 секунд", Label = "Секунда" };
        yield return new InputTime() { Text = "через 20 минут", Label = "Минута" };
        yield return new InputTime() { Text = "10:00", Label = "Время" };
    }
}
