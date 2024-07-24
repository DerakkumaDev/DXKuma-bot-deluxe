namespace DXKumaBot.Bot.Message;

public sealed record MediaMessage(MediaType Type, string Path)
{
    public static implicit operator MessagePair(MediaMessage message)
    {
        return new(message);
    }
}