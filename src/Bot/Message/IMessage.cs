namespace DXKumaBot.Bot.Message;

public class MessagePair
{
    public TextMessage? Text { get; }
    public MediaMessage? Media { get; }
    
    public MessagePair(TextMessage message)
    {
        Text = message;
    }
    
    public MessagePair(TextMessage message, MediaMessage media)
    {
        Text = message;
        Media = media;
    }
    
    public MessagePair(MediaMessage media)
    {
        Media = media;
    }
}