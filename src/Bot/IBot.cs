using DXKumaBot.Bot.Message;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot;

public interface IBot
{
    Task SendMessageAsync(MessagePair messages, BotMessage source, bool noReply);
}