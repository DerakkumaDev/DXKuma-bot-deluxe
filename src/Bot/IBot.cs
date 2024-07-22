using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using Lagrange.Core.Event.EventArg;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot;

public interface IBot
{
    Task SendMessageAsync(MessageReceivedEventArgs messageToReply, MessagePair messages,
        Possible<GroupMessageEvent, TgMessage>? source);
}