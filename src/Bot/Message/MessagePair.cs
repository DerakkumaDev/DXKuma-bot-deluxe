namespace DXKumaBot.Bot.Message;

public sealed class MessagePair
{
    public MessagePair(TextMessage message, MediaMessage media)
    {
        Text = message;
        Media = media;
    }

    public MessagePair(TextMessage message)
    {
        Text = message;
    }

    public MessagePair(MediaMessage media)
    {
        Media = media;
    }

    public TextMessage? Text { get; }
    public MediaMessage? Media { get; }
}