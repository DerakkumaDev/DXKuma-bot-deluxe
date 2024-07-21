namespace DXKumaBot.Bot.Message;

public class MediaMessage(MediaType type, MemoryStream stream)
{
    public MediaType Type { get; } = type;

    public MemoryStream Data { get; } = stream;
}