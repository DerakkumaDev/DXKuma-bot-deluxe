namespace DXKumaBot.Bot.Message;

public class TextMessage
{
    public string Text { get; set; }

    public static implicit operator TextMessage(string text)
    {
        return new()
        {
            Text = text
        };
    }

    public static implicit operator string(TextMessage message)
    {
        return message.Text;
    }

    public static implicit operator MessagePair(TextMessage message)
    {
        return new(message);
    }
}