using DXKumaBot.Bot.Message;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DXKumaBot.Bot.Telegram;

public class TgBot : IBot
{
    private readonly TelegramBotClient _bot;
    public TgBot()
    {
        // _bot = new TelegramBotClient();
    }
    public async Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SendMessageAsync(long id, MessagePair messages, int? threadId = null)
    {
        if (messages.Media is null)
        {
            await _bot.SendTextMessageAsync(id, messages.Text!.Text, threadId, ParseMode.MarkdownV2);
            return;
        }

        InputFileStream file = InputFile.FromStream(messages.Media.Data);
        switch (messages.Media.Type)
        {
            case MediaType.Audio:
                await _bot.SendAudioAsync(id, file, threadId, messages.Text?.Text, ParseMode.MarkdownV2);
                break;
            case MediaType.Photo:
                await _bot.SendPhotoAsync(id, file, threadId, messages.Text?.Text, ParseMode.MarkdownV2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(messages));
        }
    }

    public async Task SendMessageAsync(MessageReceivedEventArgs messageToReply, MessagePair messages)
    {
        if (messageToReply.TgMessage is null)
        {
            throw new ArgumentNullException(nameof(messageToReply));
        }

        await SendMessageAsync(messageToReply.TgMessage.MessageId, messages);
    }
}