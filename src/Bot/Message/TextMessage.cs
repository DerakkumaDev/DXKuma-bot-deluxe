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
}