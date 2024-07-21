using DXKumaBot.Bot;
using DXKumaBot.Bot.Message;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public static partial class LoveYou
{
    public static void Register()
    {
        BotInstance.MessageReceived += Main;
    }

    private static async Task Main(object? sender, MessageReceivedEventArgs args)
    {
        if (MessageRegex().IsMatch(args.Text))
        {
            return;
        }

        TextMessage textMessage = new()
        {
            Text = "迪拉熊也喜欢你❤️"
        };
        MessagePair messages = new(textMessage);
        await args.Reply(messages);
    }

    [GeneratedRegex("^(迪拉熊|dlx)我喜欢你$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex MessageRegex();
}