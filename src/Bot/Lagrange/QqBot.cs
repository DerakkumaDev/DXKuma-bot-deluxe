using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DXKumaBot.Bot.Lagrange;

public class QQBot : IBot
{
    private readonly BotContext _bot;
    private readonly BotKeystore? _keyStore;

    public QQBot()
    {
        BotDeviceInfo deviceInfo = GetDeviceInfo();
        _keyStore = LoadKeystore();

        _bot = BotFactory.Create(new()
        {
            UseIPv6Network = false,
            GetOptimumServer = true,
            AutoReconnect = true,
            Protocol = Protocols.Linux,
            CustomSignProvider = new OneBotSigner()
        }, deviceInfo, _keyStore ?? new BotKeystore());
    }

    public async Task RunAsync()
    {
        if (_keyStore is null)
        {
            Console.WriteLine("try fetching qr code");
            (string Url, byte[] QrCode)? qrCode = await _bot.FetchQrCode();
            if (qrCode is null)
            {
                return;
            }

            await File.WriteAllBytesAsync("qr.png", qrCode.Value.QrCode);
            await _bot.LoginByQrCode();
        }
        else
        {
            await _bot.LoginByPassword();
        }
    }

    private static BotDeviceInfo GetDeviceInfo()
    {
        if (!File.Exists("Config/DeviceInfo.json"))
        {
            BotDeviceInfo deviceInfo = BotDeviceInfo.GenerateInfo();
            File.WriteAllText("Config/DeviceInfo.json", JsonSerializer.Serialize(deviceInfo));
            return deviceInfo;
        }

        string text = File.ReadAllText("Config/DeviceInfo.json");
        BotDeviceInfo? info = JsonSerializer.Deserialize<BotDeviceInfo>(text);
        if (info is not null)
        {
            return info;
        }

        info = BotDeviceInfo.GenerateInfo();
        File.WriteAllText("Config/DeviceInfo.json", JsonSerializer.Serialize(info));

        return info;
    }

    private static void SaveKeystore(BotKeystore keystore)
    {
        File.WriteAllText("Config/Keystore.json", JsonSerializer.Serialize(keystore));
    }

    private static BotKeystore? LoadKeystore()
    {
        if (!File.Exists("Config/Keystore.json"))
        {
            return null;
        }

        string text = File.ReadAllText("Config/Keystore.json");
        return JsonSerializer.Deserialize<BotKeystore>(text, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });
    }
}