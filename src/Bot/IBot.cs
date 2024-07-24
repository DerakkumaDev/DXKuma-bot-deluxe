using DXKumaBot.Bot.Message;

namespace DXKumaBot.Bot;

public interface IBot
{
    Task<BotMessage> SendMessageAsync(MessagePair messages, BotMessage source, bool noReply);
    Task SendMessageAsync(MessagePair messages, long id, TgMessage? msg);
    Task DeleteMessageAsync(BotMessage message);
    Task<string> GetUserName(long userId, long chatId);
}