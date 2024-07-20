using DXKumaBot.Bot.Message;

namespace DXKumaBot.Bot;

public interface IBot
{ 
    Task SendMessageAsync(MessageReceivedEventArgs messageToReply, MessagePair messages);
}