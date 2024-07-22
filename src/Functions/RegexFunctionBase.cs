using DXKumaBot.Bot;
using DXKumaBot.Bot.EventArg;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public abstract class RegexFunctionBase : IFunction
{
    public void Register()
    {
        BotInstance.MessageReceived += async (sender, args) =>
        {
            if (!MessageRegex().IsMatch(args.Message.Text))
            {
                return;
            }

            await Main(sender, args);
        };
    }

    private protected abstract Task Main(object? sender, MessageReceivedEventArgs args);

    private protected abstract Regex MessageRegex();
}