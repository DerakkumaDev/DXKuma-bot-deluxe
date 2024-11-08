using Lagrange.Core;

namespace DXKumaBot.Bot.EventArg;

public abstract class BotEventArgsBase(BotContext bot) : EventArgs
{
    public BotContext Bot => bot;
}