using DXKumaBot.Bot.EventArg;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public abstract class RegexFunctionBase
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Message.Text))
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Message.Text))
        {
            return;
        }

        await MainAsync(sender, args);
    }

    private protected abstract Task MainAsync(object? sender, MessageReceivedEventArgs args);

    private protected abstract Regex MessageRegex();
}