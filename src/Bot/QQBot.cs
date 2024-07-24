using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;

namespace DXKumaBot.Bot;

public sealed class QqBot : IBot
{
    private readonly BotContext _bot;
    private readonly BotKeystore? _keyStore;

    public QqBot()
    {
        BotDeviceInfo deviceInfo = LagrangeHelper.GetDeviceInfo();
        _keyStore = LagrangeHelper.LoadKeystore();
        _bot = BotFactory.Create(new(), deviceInfo, _keyStore ?? new BotKeystore());
    }

    public uint Id => _bot.BotUin;

    public async Task<BotMessage> SendMessageAsync(MessagePair messages, BotMessage source, bool noReply)
    {
        return new(this,
            (await SendMessageAsync(Convert.ToUInt32(source.ChatId), messages.Text!, messages.Media,
                noReply ? default : source.QqMessage))!);
    }

    public async Task SendMessageAsync(MessagePair messages, long id, TgMessage? _)
    {
        await SendMessageAsync(Convert.ToUInt32(id), messages.Text!, messages.Media);
    }

    public async Task DeleteMessageAsync(BotMessage message)
    {
        if (await _bot.RecallGroupMessage(message.QqMessage!))
        {
            return;
        }

        throw new OperationCanceledException();
    }

    public async Task<string> GetUserName(long id, long _)
    {
        BotUserInfo? userInfo = await _bot.FetchUserInfo(Convert.ToUInt32(id));
        if (userInfo is null)
        {
            throw new ArgumentException($"{id} not a valid user ID", nameof(id));
        }

        return userInfo.Nickname;
    }

    public event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event AsyncEventHandler<PokedEventArgs>? Poked;
    public event AsyncEventHandler<MembersAddedEventArgs>? MembersAdded;
    public event AsyncEventHandler<MembersLeftEventArgs>? MembersLeft;

    public async Task RunAsync()
    {
        RegisterEvents();
        if (_keyStore is null)
        {
            (string Url, byte[] QrCode)? qrCode = await _bot.FetchQrCode();
            if (qrCode is null)
            {
                throw new NotSupportedException();
            }

            await File.WriteAllBytesAsync("qr.png", qrCode.Value.QrCode);
            await _bot.LoginByQrCode();
            LagrangeHelper.SaveKeystore(_bot.UpdateKeystore());
            return;
        }

        await _bot.LoginByPassword();
    }

    public async Task<BotUserInfo?> GetUserInfoAsync(uint userId)
    {
        return await _bot.FetchUserInfo(userId);
    }

    private void RegisterEvents()
    {
#if DEBUG
        _bot.Invoker.OnBotCaptchaEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOfflineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOnlineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotNewDeviceVerify += (_, @event) => { Console.WriteLine(@event.ToString()); };
#endif
        _bot.Invoker.OnGroupMessageReceived += async (sender, args) =>
        {
            if (MessageReceived is null)
            {
                return;
            }

            await MessageReceived.Invoke(sender, new(this, args));
        };
        _bot.Invoker.OnGroupPokeEvent += async (sender, args) =>
        {
            if (Poked is null)
            {
                return;
            }

            await Poked.Invoke(sender, new(this, args));
        };
        _bot.Invoker.OnGroupMemberIncreaseEvent += async (sender, args) =>
        {
            if (MembersAdded is null)
            {
                return;
            }

            await MembersAdded.Invoke(sender, new(this, args));
        };
        _bot.Invoker.OnGroupMemberDecreaseEvent += async (sender, args) =>
        {
            if (MembersLeft is null)
            {
                return;
            }

            await MembersLeft.Invoke(sender, new(this, args));
        };
    }

    private async Task<MessageChain?> SendMessageAsync(uint? id, string? text = null, MediaMessage? media = null,
        MessageChain? source = null)
    {
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        MessageBuilder messageBuilder = MessageBuilder.Group(id.Value);
        if (source is not null)
        {
            messageBuilder.Forward(source);
        }

        if (text is not null)
        {
            messageBuilder.Text(text);
        }

        if (media is not null)
        {
            byte[] data = await File.ReadAllBytesAsync(media.Path);
            switch (media.Type)
            {
                case MediaType.Audio:
                    messageBuilder.Record(data);
                    break;
                case MediaType.Photo:
                    messageBuilder.Image(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(media));
            }
        }

        MessageResult replyResult = await _bot.SendMessage(messageBuilder.Build());
        List<MessageChain>? replyMessages =
            await _bot.GetGroupMessage(id.Value, replyResult.Sequence!.Value, replyResult.Sequence.Value);
        return replyMessages?.Count is 1 ? replyMessages[0] : default;
    }
}