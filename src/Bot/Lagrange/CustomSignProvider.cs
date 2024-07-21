// From https://github.com/LagrangeDev/Lagrange.Core/blob/master/Lagrange.OneBot/Utility/OneBotSigner.cs

using DXKumaBot.Utils;
using Lagrange.Core.Utility.Sign;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace DXKumaBot.Bot.Lagrange;

public class CustomSignProvider : SignProvider
{
    private readonly HttpClient _client;
    private readonly string _signServer;
    private readonly Timer _timer;

    public CustomSignProvider()
    {
        _signServer = "https://sign.lagrangecore.org/api/sign";
        _client = new();
        _timer = new(_ =>
        {
            bool reconnect = Available = Test();
            if (reconnect)
            {
                _timer?.Change(-1, 5000);
            }
        });
    }

    public override byte[]? Sign(string cmd, uint seq, byte[] body, [UnscopedRef] out byte[]? ver,
        [UnscopedRef] out string? token)
    {
        ver = null;
        token = null;
        if (!WhiteListCommand.Contains(cmd))
        {
            return null;
        }

        if (!Available)
        {
            return new byte[35]; // Dummy signature
        }

        JsonObject payload = new()
        {
            { "cmd", cmd },
            { "seq", seq },
            { "src", body.Hex() }
        };

        try
        {
            HttpResponseMessage message = _client.PostAsJsonAsync(_signServer, payload).Result;
            string response = message.Content.ReadAsStringAsync().Result;
            JsonObject? json = JsonSerializer.Deserialize<JsonObject>(response);

            ver = Convert.FromHexString(json?["value"]?["extra"]?.ToString() ?? string.Empty);
            token = Encoding.ASCII.GetString(
                Convert.FromHexString(json?["value"]?["token"]?.ToString() ?? string.Empty));
            return Convert.FromHexString(json?["value"]?["sign"]?.ToString() ?? string.Empty);
        }
        catch
        {
            Available = false;
            _timer.Change(0, 5000);

            return new byte[35]; // Dummy signature
        }
    }

    public override bool Test()
    {
        try
        {
            UriBuilder uriBuilder = new($"{_signServer}/ping");
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Query = query.ToString();
            using HttpResponseMessage response = _client.GetAsync(uriBuilder.Uri).Result;
            string responseStr = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<JsonObject>(responseStr)?["code"]?.GetValue<int>() is 0;
        }
        catch
        {
            return false;
        }
    }
}