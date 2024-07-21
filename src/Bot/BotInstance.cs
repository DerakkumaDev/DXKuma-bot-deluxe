using DXKumaBot.Bot.Lagrange;
using DXKumaBot.Bot.Telegram;
using DXKumaBot.Utils;

namespace DXKumaBot.Bot;

public sealed class BotInstance
{
    private readonly QQBot _qqBot = new();
    private readonly TgBot _tgBot = new();

    public static event AsyncEventHandler<MessageReceivedEventArgs> MessageReceived;

    public async Task RunAsync()
    {
        await Task.WhenAll(_qqBot.RunAsync(), _tgBot.RunAsync());
    }

    public async Task SendMessageAsync(object message)
    {
        throw new NotImplementedException();
    }
}