using DXKumaBot.Bot.EventArg;
using DXKumaBot.Functions;
using DXKumaBot.Utils;

namespace DXKumaBot.Bot;

public sealed class BotInstance(Config config)
{
    private readonly QqBot _qqBot = new();
    private readonly TgBot _tgBot = new(config.Telegram);

    public event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event AsyncEventHandler<PokedEventArgs>? Poked;
    public event AsyncEventHandler<MembersAddedEventArgs>? MembersAdded;
    public event AsyncEventHandler<MembersLeftEventArgs>? MembersLeft;

    private void RegisterFunctions()
    {
        LoveYou loveYou = new();
        WannaCao wannaCao = new();
        Cum cum = new();
        Roll roll = new();
        EatBreak eatBreak = new();
        Repeater repeater = new();
        Poke poke = new();
        MemberChange memberChange = new();
        Help help = new();

        MessageReceived += loveYou.EntryAsync;
        MessageReceived += wannaCao.EntryAsync;
        MessageReceived += cum.EntryAsync;
        MessageReceived += roll.EntryAsync;
        MessageReceived += eatBreak.EntryAsync;
        MessageReceived += repeater.EntryAsync;
        Poked += poke.QqEntryAsync;
        MessageReceived += poke.EntryAsync;
        MembersAdded += memberChange.JoinEntryAsync;
        MembersLeft += memberChange.LeftEntryAsync;
        MessageReceived += help.EntryAsync;
    }

    private void RegisterEvents()
    {
        _qqBot.MessageReceived += MessageReceived;
        _qqBot.Poked += Poked;
        _qqBot.MembersAdded += MembersAdded;
        _qqBot.MembersLeft += MembersLeft;

        _tgBot.MessageReceived += MessageReceived;
        _tgBot.MembersAdded += MembersAdded;
        _tgBot.MembersLeft += MembersLeft;
    }

    public async Task RunAsync()
    {
        RegisterFunctions();
        RegisterEvents();
        _tgBot.Run();
        await _qqBot.RunAsync();
    }
}