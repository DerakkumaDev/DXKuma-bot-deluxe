using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public sealed partial class LoveYou : RegexFunctionBase
{
    protected override async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        TextMessage message = new()
        {
            Text = "迪拉熊也喜欢你❤️"
        };
        await args.Reply(message);
    }

    [GeneratedRegex("^(迪拉熊|dlx)我喜欢你$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline)]
    protected override partial Regex MessageRegex();
}