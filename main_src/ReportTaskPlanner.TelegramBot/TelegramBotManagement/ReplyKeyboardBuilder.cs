using PRTelegramBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReportTaskPlanner.TelegramBot.TelegramBotManagement;

public sealed class ReplyKeyboardBuilder
{
    private readonly List<KeyboardButton> _buttons;

    public IReadOnlyCollection<KeyboardButton> Buttons => _buttons;

    private ReplyKeyboardBuilder(List<KeyboardButton> buttons) => _buttons = buttons;

    public ReplyKeyboardBuilder() => _buttons = [];

    public ReplyKeyboardBuilder AddButton(string buttonName)
    {
        _buttons.Add(new KeyboardButton(buttonName));
        return new ReplyKeyboardBuilder(_buttons);
    }

    public ReplyKeyboardMarkup Build(int columns) => MenuGenerator.ReplyKeyboard(columns, _buttons);
}
