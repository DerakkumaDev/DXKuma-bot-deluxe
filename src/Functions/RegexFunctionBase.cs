using DXKumaBot.Bot;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public abstract class RegexFunctionBase
{
    public void Register()
    {
        BotInstance.MessageReceived += async (sender, args) =>
        {
            if (!MessageRegex().IsMatch(args.Text))
            {
                return;
            }

            await Main(sender, args);
        };
    }

    protected abstract Task Main(object? sender, MessageReceivedEventArgs args);

    protected abstract Regex MessageRegex();
}