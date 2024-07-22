using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;

namespace DXKumaBot.Functions;

public sealed class Poke : IFunction
{
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

    private readonly int[] _weights = [9, 9, 9, 9, 9, 9, 9, 9, 4, 4];


    public void Register()
    {
        BotInstance.Poked += async (_, args) =>
        {
            int index = Random.Shared.Choose(_weights);
            string reply = _replies[index];
            string filePath = Path.Combine("Static", nameof(Poke), $"{index}.png");
            MediaMessage message = new(MediaType.Photo, filePath);
            await args.Message.Reply(new(reply, message));
        };
    }
}