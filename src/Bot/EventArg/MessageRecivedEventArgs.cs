using Lagrange.Core;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message.Entity;

namespace DXKumaBot.Bot.EventArg;

public sealed class MessageReceivedEventArgs(BotContext bot, GroupMessageEvent message) : BotEventArgsBase(bot)
{
    public GroupMessageEvent Event => message;
    public Lazy<string?> Text => new(() => Event.Chain.GetEntity<TextEntity>()?.Text);
    public uint GroupUin => Event.Chain.GroupUin!.Value;
    public uint TargetUin => Event.Chain.TargetUin;
    public DateTime DateTime => Event.Chain.Time;
    public bool MentionedBot => Event.Chain.TargetUin == Bot.BotUin;
}