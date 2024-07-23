using Lagrange.Core.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DXKumaBot.Utils;

public static class LagrangeHelper
{
    public static BotDeviceInfo GetDeviceInfo()
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

    public static void SaveKeystore(BotKeystore keystore)
    {
        File.WriteAllText("Keystore.json", JsonSerializer.Serialize(keystore));
    }

    public static BotKeystore? LoadKeystore()
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
}