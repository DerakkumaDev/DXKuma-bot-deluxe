using DXKumaBot.Bot.Message;
using DXKumaBot.Utils;
using Lagrange.Core.Event.EventArg;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using TgMessage = Telegram.Bot.Types.Message;

namespace DXKumaBot.Bot.Telegram;

public class TgBot(TelegramConfig config) : IBot
{
    private readonly TelegramBotClient _bot = new(config.BotToken,
        config.Proxy.Enabled ? new(new HttpClientHandler { Proxy = new WebProxy(config.Proxy.Url, true) }) : default);

    public async Task SendMessageAsync(MessageReceivedEventArgs messageToReply, MessagePair messages,
        Possible<GroupMessageEvent, TgMessage> source)
    {
        if (messageToReply.TgMessage is null)
        {
            throw new ArgumentNullException(nameof(messageToReply));
        }

        await SendMessageAsync(messageToReply.TgMessage.Chat.Id, messages, source: source);
    }

    public event Utils.AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;

    public void Run()
    {
        _bot.StartReceiving(async (bot, update, _) =>
        {
            if (update is not
                {
                    Type: UpdateType.Message,
                    Message:
                    {
                        Type: MessageType.Text,
                        Text: not null
                    }
                } || MessageReceived is null)
            {
                return;
            }

            await MessageReceived.Invoke(bot, new(this, update.Message));
        }, (_, e, _) => { Console.WriteLine(e); });
    }

    private async Task SendMessageAsync(long id, MessagePair messages, int? threadId = null, TgMessage? source = null)
    {
        if (messages.Media is null)
        {
            await _bot.SendTextMessageAsync(id, messages.Text!.Text, threadId,
                replyParameters: source is null ? default(ReplyParameters) : source);
            return;
        }

        InputFile file = InputFile.FromStream(File.OpenRead(messages.Media.Path));
        switch (messages.Media.Type)
        {
            case MediaType.Audio:
                await _bot.SendAudioAsync(id, file, threadId, messages.Text?.Text,
                    replyParameters: source is null ? default(ReplyParameters) : source);
                break;
            case MediaType.Photo:
                await _bot.SendPhotoAsync(id, file, threadId, messages.Text?.Text,
                    replyParameters: source is null ? default(ReplyParameters) : source);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(messages));
        }
    }
}