using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Interaction;

public sealed partial class Poke : RegexFunctionBase
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

    public async Task QqEntryAsync(object? sender, PokedEventArgs args)
    {
        if (!args.Message.ToBot)
        {
            return;
        }

        MessagePair messages = GetReplyMessages();
        await args.Message.ReplyAsync(messages, true);
    }

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        MessagePair messages = GetReplyMessages();
        await args.Message.ReplyAsync(messages);
    }

    private MessagePair GetReplyMessages()
    {
        int index = _random.Next();
        string reply = _replies[index];
        string filePath = Path.Combine("Static", nameof(Poke), $"{index}.png");
        MediaMessage message = new(MediaType.Photo, filePath);
        return new(reply, message);
    }

    [GeneratedRegex("^戳屁(屁|股)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}