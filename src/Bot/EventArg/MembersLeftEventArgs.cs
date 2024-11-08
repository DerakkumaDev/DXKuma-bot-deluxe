using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;

namespace DXKumaBot.Bot.EventArg;

public sealed class MembersLeftEventArgs(BotContext bot, GroupMemberDecreaseEvent message) : BotEventArgsBase(bot)
{
    public GroupMemberDecreaseEvent Event => message;
    public string? MemberQid => MemberInfo.Value.Qid;
    public string MemberName => MemberInfo.Value.Nickname;

    private Lazy<BotUserInfo> MemberInfo => new(() => Bot.FetchUserInfo(Event.MemberUin).
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            Result
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        !);
}