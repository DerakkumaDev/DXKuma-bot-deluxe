using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace DXKumaBot.Functions.Interaction;

public sealed class Repeater
{
    private readonly Dictionary<uint, (string, int)> _lastMessages = new();

    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (!args.Event.Chain.All(x => x is TextEntity))
        {
            _lastMessages.Remove(args.GroupUin, out _);
            return;
        }

        if (!_lastMessages.TryGetValue(args.GroupUin, out (string Text, int Times) lastMessage) ||
            lastMessage.Text != args.Text.Value)
        {
            _lastMessages[args.GroupUin] = (args.Text.Value!, 1);
            return;
        }

        if (lastMessage.Times > 2)
        {
            return;
        }

        ++lastMessage.Times;
        if (lastMessage.Times is 3)
        {
            MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
            messageBuilder.Text(args.Text.Value);
            await args.Bot.SendMessage(messageBuilder.Build());
        }

        _lastMessages[args.GroupUin] = lastMessage;
    }
}