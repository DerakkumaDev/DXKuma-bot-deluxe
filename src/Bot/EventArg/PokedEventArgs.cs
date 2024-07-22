using DXKumaBot.Bot.Message;
using Lagrange.Core.Event.EventArg;

namespace DXKumaBot.Bot.EventArg;

public sealed class PokedEventArgs(QqBot bot, GroupPokeEvent message) : BotEventArgsBase
{
    public override BotMessage Message { get; } = new(bot, message.GroupUin);
}