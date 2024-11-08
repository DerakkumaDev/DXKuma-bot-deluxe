using DXKumaBot.Utils;
using Lagrange.Core.Message;
using System.Diagnostics.CodeAnalysis;

namespace DXKumaBot.Functions.Gallery;

public sealed record RakingData : IComparable<RakingData>
{
    private readonly Queue<DateTime> _dates;
    public required uint Id { get; init; }

    public required Queue<DateTime> Dates
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

    public static void Update(MessageChain message)
    {
        Storage storage = Storage.GetFromName(nameof(Functions));
        RakingData data = storage.Get<RakingData>(nameof(Gallery), message.FriendUin) ?? new()
        {
            Id = message.GroupUin!.Value,
            Dates = []
        };
        data.Dates.Enqueue(message.Time.Date);
        storage.Set(nameof(Gallery), data);
    }
}