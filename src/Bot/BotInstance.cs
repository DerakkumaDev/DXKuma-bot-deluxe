using DXKumaBot.Bot.EventArg;
using DXKumaBot.Functions;
using DXKumaBot.Utils;

namespace DXKumaBot.Bot;

public sealed class BotInstance(Config config)
{
    private readonly QqBot _qqBot = new();
    private readonly TgBot _tgBot = new(config.Telegram);

    public static event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;
    public static event AsyncEventHandler<PokedEventArgs>? Poked;

    private static void RegisterFunctions()
    {
        LoveYou loveYou = new();
        WannaCao wannaCao = new();
        Cum cum = new();
        Roll roll = new();
        EatBreak eatBreak = new();
        Repeater repeater = new();
        Poke poke = new();

        loveYou.Register();
        wannaCao.Register();
        cum.Register();
        roll.Register();
        eatBreak.Register();
        repeater.Register();
        poke.Register();
    }

    private void RegisterEvents()
    {
        _qqBot.MessageReceived += MessageReceived;
        _qqBot.Poked += Poked;
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