using DXKumaBot.Bot.EventArg;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions.Gallery;

public sealed partial class PickNsfw : RegexFunctionBase
{
    private readonly Dictionary<long, Queue<DateTime>> _groups = new();
    private readonly uint[] _specialGroups = [];

    static PickNsfw()
    {
        SpecialGroup = 0;
    }

    public static uint SpecialGroup { private get; set; }

    private protected override async Task MainAsync(object? sender, MessageReceivedEventArgs args)
    {
        MessageBuilder messageBuilder = MessageBuilder.Group(args.GroupUin);
        string dirPath = Path.Combine("Static", nameof(Gallery), "NSFW");
        string[] files = Directory.GetFiles(dirPath);
        if (!_specialGroups.Contains(args.GroupUin) || files.Length <= 0)
        {
            messageBuilder.Text("迪拉熊不准你看");
            string picPath = Path.Combine("Static", nameof(Gallery), "0.png");
            messageBuilder.Image(picPath);
            await args.Bot.SendMessage(messageBuilder.Build());
            return;
        }

        if (args.GroupUin != SpecialGroup && !Pick.CheckInterval(_groups, args.GroupUin, args.DateTime))
        {
            messageBuilder.Text("迪拉熊关心你的身体健康，所以图就先不发了~");
            await args.Bot.SendMessage(messageBuilder.Build());
            return;
        }

        int index = Random.Shared.Next(files.Length);
        messageBuilder.Image(files[index]);
        MessageResult message = await args.Bot.SendMessage(messageBuilder.Build());
        await Task.Delay(TimeSpan.FromSeconds(10));
        await args.Bot.RecallGroupMessage(args.GroupUin, message);
    }

    [GeneratedRegex("^(随机迪拉熊|dlx)((涩|色|瑟)图|st)$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private protected override partial Regex MessageRegex();
}