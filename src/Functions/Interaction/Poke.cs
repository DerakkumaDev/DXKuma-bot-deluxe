using DXKumaBot.Bot.EventArg;
using DXKumaBot.Utils;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace DXKumaBot.Functions.Interaction;

public sealed class Poke
{
    private readonly WeightsRandom _random = new([9, 9, 9, 9, 9, 9, 9, 9, 4, 4]);

    private readonly string[] _replies =
    [
        "不可以戳迪拉熊的屁股啦~",
        "你怎么能戳迪拉熊的屁股！",
        "为什么要戳迪拉熊的屁股呢？",
        "再戳我屁股迪拉熊就不跟你玩了！",
        "你再戳一个试试！",
        "讨厌啦~不要戳迪拉熊的屁股啦~",
        "你觉得戳迪拉熊的屁股很好玩吗？",
        "不许戳迪拉熊的屁股啦！",
        "迪拉熊懂你的意思~",
        "再戳迪拉熊就跟你绝交！"
    ];

    public async Task EntryAsync(object? sender, PokedEventArgs args)
    {
        if (!args.ToBot)
        {
            return;
        }

        MessageBuilder messageBuilder = MessageBuilder.Group(args.Event.GroupUin);
        int index = _random.Next();
        string reply = _replies[index];
        messageBuilder.Text(reply);
        string filePath = Path.Combine("Static", GetType().Name, $"{index}.png");
        messageBuilder.Image(filePath);
        await args.Bot.SendMessage(messageBuilder.Build());
    }
}