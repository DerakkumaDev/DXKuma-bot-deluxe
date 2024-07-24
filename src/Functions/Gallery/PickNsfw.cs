using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class PickNsfw : RegexFunctionBase
{
    private readonly ConcurrentDictionary<long, ConcurrentQueue<DateTime>> _groups = new();
    private readonly int _specialGroup = 967611986;
    private readonly int[] _specialGroups = [236030263, 938593095, 783427193];

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (!_specialGroups.Contains((int)args.Message.ChatId))
        {
            string picPath = Path.Combine("Static", nameof(Gallery), "0.png");
            MediaMessage picMessage = new(MediaType.Photo, picPath);
            await args.Message.ReplyAsync(new("迪拉熊不准你看", picMessage));
            return;
        }


        if (args.Message.ChatId != _specialGroup &&
            !Pick.CheckInterval(_groups, args.Message.ChatId, args.Message.DateTime))
        {
            await args.Message.ReplyAsync(new("迪拉熊关心你的身体健康，所以图就先不发了~"));
            return;
        }

        string dirPath = Path.Combine("Static", nameof(Gallery), "NSFW");
        string[] files = Directory.GetFiles(dirPath);
        int index = Random.Shared.Next(files.Length);
        MediaMessage message = new(MediaType.Photo, files[index]);
        BotMessage reply = await args.Message.ReplyAsync(message);
        await Task.Delay(TimeSpan.FromSeconds(10));
        await reply.Bot.DeleteMessageAsync(reply);
    }

    [GeneratedRegex("^(随机迪拉熊|dlx)((涩|色|瑟)图|st)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}