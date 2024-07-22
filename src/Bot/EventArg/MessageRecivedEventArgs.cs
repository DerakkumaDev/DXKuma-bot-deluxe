using DXKumaBot.Bot.Message;
using Lagrange.Core.Event.EventArg;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot.EventArg;

public sealed class MessageReceivedEventArgs : BotEventArgsBase
{
    public MessageReceivedEventArgs(IBot bot, GroupMessageEvent message)
    {
        Message = new(bot, message.Chain);
    }

    public MessageReceivedEventArgs(IBot bot, TgMessage message)
    {
        Message = new(bot, message);
    }

    public override BotMessage Message { get; }
}