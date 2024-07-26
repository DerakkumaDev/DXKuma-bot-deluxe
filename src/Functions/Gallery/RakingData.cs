using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace DXKumaBot.Functions.Gallery;

public sealed record RakingData : IComparable<RakingData>
{
    private readonly ConcurrentQueue<DateTime> _dates;
    public required long Id { get; init; }
    public required MessageSource SourceType { get; init; }

    public required ConcurrentQueue<DateTime> Dates
    {
        get
        {
            while (_dates.TryPeek(out DateTime date))
            {
                if (date.IsSameWeek(DateTime.Today))
                {
                    break;
                }

                _dates.TryDequeue(out _);
            }

            return _dates;
        }
        [MemberNotNull(nameof(_dates))] init => _dates = value;
    }

    public int CompareTo(RakingData? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        return Dates.Count.CompareTo(other.Dates.Count);
    }

    public static void Update(BotMessage message)
    {
        RakingData data = Storage.Get<RakingData>(nameof(Gallery), message.UserId) ?? new()
        {
            Id = message.ChatId,
            SourceType = message.SourceType,
            Dates = []
        };
        data.Dates.Enqueue(message.DateTime.Date);
        Storage.Set(nameof(Gallery), data);
    }
}