using DXKumaBot.Bot.Message;

namespace DXKumaBot.Bot.EventArg;

public abstract class BotEventArgsBase : EventArgs
{
    public abstract BotMessage Message { get; }
}