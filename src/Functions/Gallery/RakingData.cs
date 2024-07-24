namespace DXKumaBot.Functions.Gallery;

public sealed record RakingData
{
    public required long Id { get; init; }
    public required DateTime Date { get; init; }
    public required Dictionary<long, int> Counts { get; init; }
}