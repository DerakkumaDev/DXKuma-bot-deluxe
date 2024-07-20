namespace DXKumaBot.Bot.Message;

public class MediaMessage(MediaType type, Stream stream)
{
    public MediaType Type { get; } = type;
    
    public Stream Data { get; } = stream;
}