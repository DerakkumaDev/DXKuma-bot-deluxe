using DXKumaBot.Bot.Lagrange;
using DXKumaBot.Bot.Telegram;
using DXKumaBot.Functions;
using DXKumaBot.Utils;

namespace DXKumaBot.Bot;

public sealed class BotInstance(Config config)
{
    private readonly QqBot _qqBot = new();
    private readonly TgBot _tgBot = new(config.Telegram);

    public static event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;

    private static void RegisterFunctions()
    {
        LoveYou loveYou = new();
        loveYou.Register();
    }

    private void RegisterEvents()
    {
        _qqBot.MessageReceived += MessageReceived;
        _tgBot.MessageReceived += MessageReceived;
    }

    public async Task RunAsync()
    {
        RegisterFunctions();
        RegisterEvents();
        _tgBot.Run();
        await _qqBot.RunAsync();
    }
}