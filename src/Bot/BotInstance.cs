using DXKumaBot.Bot.Lagrange;
using DXKumaBot.Bot.Telegram;

namespace DXKumaBot.Bot;

public sealed class BotInstance
{
    private readonly QQBot _qqBot = new();
    private readonly TgBot _tgBot = new();

    public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

    public async Task RunAsync()
    {
        await Task.WhenAll(_qqBot.RunAsync(), _tgBot.RunAsync());
    }

    public async Task SendMessageAsync(object message)
    {
        throw new NotImplementedException();
    }
}