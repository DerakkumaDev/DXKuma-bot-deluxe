using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DXKumaBot.Bot;

public sealed class QqBot : IBot
{
    private readonly BotContext _bot;
    private readonly BotKeystore? _keyStore;

    public QqBot()
    {
        BotDeviceInfo deviceInfo = GetDeviceInfo();
        _keyStore = LoadKeystore();
        _bot = BotFactory.Create(new(), deviceInfo, _keyStore ?? new BotKeystore());
    }

    public async Task SendMessageAsync(MessagePair messages, BotMessage source, bool noReply)
    {
        await SendMessageAsync(Convert.ToUInt32(source.ChatId), messages.Text!, messages.Media,
            noReply ? default : source.QqMessage);
    }

    public async Task SendMessageAsync(MessagePair messages, long id, TgMessage? _)
    {
        await SendMessageAsync(Convert.ToUInt32(id), messages.Text!, messages.Media);
    }

    public event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event AsyncEventHandler<PokedEventArgs>? Poked;
    public event AsyncEventHandler<MembersAddedEventArgs>? MembersAdded;
    public event AsyncEventHandler<MembersLeftEventArgs>? MembersLeft;

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
            SaveKeystore(_bot.UpdateKeystore());
            return;
        }

        await _bot.LoginByPassword();
    }


    public async Task<BotUserInfo?> GetUserInfo(uint userId)
    {
        return await _bot.FetchUserInfo(userId);
    }

    private static BotDeviceInfo GetDeviceInfo()
    {
        if (!File.Exists("DeviceInfo.json"))
        {
            BotDeviceInfo deviceInfo = BotDeviceInfo.GenerateInfo();
            File.WriteAllText("DeviceInfo.json", JsonSerializer.Serialize(deviceInfo));
            return deviceInfo;
        }

        string text = File.ReadAllText("DeviceInfo.json");
        BotDeviceInfo? info = JsonSerializer.Deserialize<BotDeviceInfo>(text);
        if (info is not null)
        {
            return info;
        }

        info = BotDeviceInfo.GenerateInfo();
        File.WriteAllText("DeviceInfo.json", JsonSerializer.Serialize(info));
        return info;
    }

    private static void SaveKeystore(BotKeystore keystore)
    {
        File.WriteAllText("Keystore.json", JsonSerializer.Serialize(keystore));
    }

    private static BotKeystore? LoadKeystore()
    {
        if (!File.Exists("Keystore.json"))
        {
            return null;
        }

        string text = File.ReadAllText("Keystore.json");
        return JsonSerializer.Deserialize<BotKeystore>(text, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });
    }

    private async Task SendMessageAsync(uint? id, string? text = null, MediaMessage? media = null,
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

        await _bot.SendMessage(messageBuilder.Build());
    }
}