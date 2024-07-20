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

public class OneBotSigner : SignProvider
{
    private readonly HttpClient _client;
    private readonly string _signServer;
    private readonly Timer _timer;

    public OneBotSigner()
    {
        _signServer = "https://sign.lagrangecore.org/api/sign";
        _client = new();

        if (string.IsNullOrEmpty(_signServer))
        {
            Available = false;
            Console.WriteLine("Signature Service is not available, login may be failed");
        }
        else
        {
            Console.WriteLine("Signature Service is successfully established");
        }

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

        if (!Available || string.IsNullOrEmpty(_signServer))
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

            Console.WriteLine("Failed to get signature, using dummy signature");
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
            if (JsonSerializer.Deserialize<JsonObject>(responseStr)?["code"]?.GetValue<int>() is not 0)
            {
                return false;
            }

            Console.WriteLine("Reconnected to Signature Service successfully");
            return true;
        }
        catch
        {
            return false;
        }
    }
}