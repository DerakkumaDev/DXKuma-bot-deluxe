using DXKumaBot.Bot.EventArg;
using DXKumaBot.Functions.Gallery;
using DXKumaBot.Functions.Interaction;
using DXKumaBot.Utils;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Threading;

namespace DXKumaBot.Bot;

public class BotHostedService : IHostedService
{
    private readonly BotContext _bot;
    private readonly BotKeystore _keyStore;

    public BotHostedService()
    {
        BotDeviceInfo deviceInfo = LagrangeHelper.GetDeviceInfo();
        _keyStore = LagrangeHelper.LoadKeystore() ?? new BotKeystore();
        _bot = BotFactory.Create(new(), deviceInfo, _keyStore);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterFunctions();
        RegisterEvents();
        if (_keyStore is null)
        {
            (string Url, byte[] QrCode)? qrCode = await _bot.FetchQrCode() ?? throw new NotSupportedException();
            await File.WriteAllBytesAsync("qr.png", qrCode.Value.QrCode, cancellationToken);
            await _bot.LoginByQrCode();
            LagrangeHelper.SaveKeystore(_bot.UpdateKeystore());
            return;
        }

        await _bot.LoginByPassword();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
    }

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
        Pick pick = new();
        PickNsfw pickNsfw = new();
        Rank rank = new();
        MessageReceived += loveYou.EntryAsync;
        MessageReceived += wannaCao.EntryAsync;
        MessageReceived += cum.EntryAsync;
        MessageReceived += roll.EntryAsync;
        MessageReceived += eatBreak.EntryAsync;
        MessageReceived += repeater.EntryAsync;
        Poked += poke.EntryAsync;
        MembersAdded += memberChange.JoinEntryAsync;
        MembersLeft += memberChange.LeftEntryAsync;
        MessageReceived += help.EntryAsync;
        MessageReceived += pick.EntryAsync;
        MessageReceived += pickNsfw.EntryAsync;
        MessageReceived += rank.EntryAsync;
    }

    private void RegisterEvents()
    {
#if DEBUG
        _bot.Invoker.OnBotCaptchaEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOfflineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOnlineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotNewDeviceVerify += (_, @event) => { Console.WriteLine(@event.ToString()); };
#endif
#pragma warning disable VSTHRD101
        _bot.Invoker.OnGroupMessageReceived += async (sender, args) =>
        {
            if (MessageReceived is null)
            {
                return;
            }

            await MessageReceived.InvokeAsync(sender, new(_bot, args));
        };
        _bot.Invoker.OnGroupPokeEvent += async (sender, args) =>
        {
            if (Poked is null)
            {
                return;
            }

            await Poked.InvokeAsync(sender, new(_bot, args));
        };
        _bot.Invoker.OnGroupMemberIncreaseEvent += async (sender, args) =>
        {
            if (MembersAdded is null)
            {
                return;
            }

            await MembersAdded.InvokeAsync(sender, new(_bot, args));
        };
        _bot.Invoker.OnGroupMemberDecreaseEvent += async (sender, args) =>
        {
            if (MembersLeft is null)
            {
                return;
            }

            await MembersLeft.InvokeAsync(sender, new(_bot, args));
        };
#pragma warning restore VSTHRD101
    }
}