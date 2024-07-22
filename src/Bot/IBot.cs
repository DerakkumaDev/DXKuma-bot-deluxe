using DXKumaBot.Bot.Message;

namespace DXKumaBot.Bot;

public interface IBot
{
    Task SendMessageAsync(MessagePair messages, BotMessage source, bool noReply);
    Task SendMessageAsync(MessagePair messages, long id, TgMessage? msg);
}