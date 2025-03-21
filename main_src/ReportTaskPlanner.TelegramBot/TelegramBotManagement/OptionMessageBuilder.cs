using PRTelegramBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace ReportTaskPlanner.TelegramBot.TelegramBotManagement;

public sealed class OptionMessageBuilder
{
    private readonly OptionMessage _message;

    private OptionMessageBuilder(OptionMessage message) => _message = message;

    public OptionMessageBuilder() => _message = new OptionMessage();

    public OptionMessageBuilder AddText(string text)
    {
        _message.Message = text;
        return new OptionMessageBuilder(_message);
    }

    public OptionMessageBuilder AddReplyKeyboardMarkup(ReplyKeyboardMarkup keyboard)
    {
        _message.MenuReplyKeyboardMarkup = keyboard;
        return new OptionMessageBuilder(_message);
    }

    public OptionMessage Build() => _message;
}
