using Lagrange.Core;
using Lagrange.Core.Event.EventArg;

namespace DXKumaBot.Bot.EventArg;

public sealed class PokedEventArgs(BotContext bot, GroupPokeEvent message) : BotEventArgsBase(bot)
{
    public GroupPokeEvent Event => message;
    public bool ToBot => Event.TargetUin == Bot.BotUin;
}