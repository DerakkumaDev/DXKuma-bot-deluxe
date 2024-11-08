using DXKumaBot.Bot.EventArg;
using System.Text.RegularExpressions;

namespace DXKumaBot.Functions;

public abstract class RegexFunctionBase
{
    public async Task EntryAsync(object? sender, MessageReceivedEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Text.Value))
        {
            return;
        }

        if (!MessageRegex().IsMatch(args.Text.Value))
        {
            return;
        }

        await MainAsync(sender, args);
    }

    private protected abstract Task MainAsync(object? sender, MessageReceivedEventArgs args);
    private protected abstract Regex MessageRegex();
}