using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;

namespace DXKumaBot.Functions.Gallery;

public sealed record RakingData
{
    public required long Id { get; init; }
    public required DateTime Date { get; init; }
    public required Dictionary<long, int> Counts { get; init; }

    public static void Update(BotMessage message)
    {
        RakingData? data = Storage.Get<RakingData>(nameof(Gallery), message.ChatId);
        if (data is null || !data.Date.IsSameWeek(DateTime.Today))
        {
            data = new()
            {
                Id = message.ChatId,
                Date = message.DateTime.Date,
                Counts = []
            };
        }

        data.Counts.TryGetValue(message.UserId, out int count);
        data.Counts[message.UserId] = count + 1;
        Storage.Set(nameof(Gallery), data);
    }
}