namespace DXKumaBot.Bot.Message;

public class MediaMessage(MediaType type, string path)
{
    public MediaType Type { get; } = type;

    public string Path { get; } = path;

    public static implicit operator MessagePair(MediaMessage message)
    {
        return new(message);
    }
}