using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DXKumaBot.Bot.Lagrange;

public class QqBot : IBot
{
    private readonly BotContext _bot;
    private readonly BotKeystore? _keyStore;

    public QqBot()
    {
        BotDeviceInfo deviceInfo = GetDeviceInfo();
        _keyStore = LoadKeystore();
        _bot = BotFactory.Create(new()
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.Linux,
            CustomSignProvider = new CustomSignProvider()
        }, deviceInfo, _keyStore ?? new BotKeystore());
    }

    public async Task SendMessageAsync(MessageReceivedEventArgs messageToReply, MessagePair messages)
    {
        await SendMessageAsync(messageToReply.QqMessage!.Chain.GroupUin, messages);
    }

    public event AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;

    private void RegisterEvents()
    {
        _bot.Invoker.OnBotCaptchaEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOfflineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotOnlineEvent += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnBotNewDeviceVerify += (_, @event) => { Console.WriteLine(@event.ToString()); };
        _bot.Invoker.OnGroupMessageReceived += async (sender, args) =>
        {
            if (MessageReceived is null)
            {
                return;
            }

            await MessageReceived.Invoke(sender, new(this, args));
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

    private async Task SendMessageAsync(uint? id, MessagePair messages)
    {
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        MessageBuilder messageBuilder = MessageBuilder.Group((uint)id);
        if (messages.Text is not null)
        {
            messageBuilder.Text(messages.Text);
        }

        if (messages.Media is not null)
        {
            byte[] data = await File.ReadAllBytesAsync(messages.Media.Path);
            switch (messages.Media.Type)
            {
                case MediaType.Audio:
                    messageBuilder.Record(data);
                    break;
                case MediaType.Photo:
                    messageBuilder.Image(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messages));
            }
        }

        await _bot.SendMessage(messageBuilder.Build());
    }
}