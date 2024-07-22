global using TgMessage = Telegram.Bot.Types.Message;
using DXKumaBot.Bot.EventArg;
using DXKumaBot.Bot.Message;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace DXKumaBot.Bot;

public sealed class TgBot(TelegramConfig config) : IBot
{
    private readonly TelegramBotClient _bot = new(config.BotToken,
        config.Proxy.Enabled ? new(new HttpClientHandler { Proxy = new WebProxy(config.Proxy.Url, true) }) : default);

    public async Task SendMessageAsync(MessagePair messages, BotMessage source, bool noReply)
    {
        await SendMessageAsync(source.ChatId, messages, source.TgMessage?.MessageThreadId,
            noReply ? default : source.TgMessage);
    }

    public async Task SendMessageAsync(MessagePair messages, long id, TgMessage? msg)
    {
        await SendMessageAsync(id, messages, msg!.MessageThreadId, msg);
    }

    public event Utils.AsyncEventHandler<MessageReceivedEventArgs>? MessageReceived;
    public event Utils.AsyncEventHandler<MembersAddedEventArgs>? MembersAdded;
    public event Utils.AsyncEventHandler<MembersLeftEventArgs>? MembersLeft;

    public void Run()
    {
        _bot.StartReceiving(async (bot, update, _) =>
        {
            if (update is { Type: UpdateType.Message, Message: { Type: MessageType.Text, Text: not null } } &&
                MessageReceived is not null)
            {
                await MessageReceived.Invoke(bot, new(this, update.Message));
            }

            if (update is
                {
                    Type: UpdateType.Message, Message: { Type: MessageType.ChatMembersAdded, NewChatMembers: not null }
                } &&
                MembersAdded is not null)
            {
                await MembersAdded.Invoke(bot, new(this, update.Message));
            }

            if (update is
                {
                    Type: UpdateType.Message, Message: { Type: MessageType.ChatMemberLeft, LeftChatMember: not null }
                } &&
                MembersLeft is not null)
            {
                await MembersLeft.Invoke(bot, new(this, update.Message));
            }
        }, (_, e, _) =>
        {
#if DEBUG
            Console.WriteLine(e);
#endif
        });
    }

    private async Task SendMessageAsync(long id, MessagePair messages, int? threadId = null, TgMessage? source = null)
    {
        if (messages.Media is null)
        {
            await _bot.SendTextMessageAsync(id, messages.Text!.Text, threadId,
                replyParameters: source is null ? default(ReplyParameters?) : source);
            return;
        }

        InputFile file = InputFile.FromStream(File.OpenRead(messages.Media.Path));
        switch (messages.Media.Type)
        {
            case MediaType.Audio:
                await _bot.SendAudioAsync(id, file, threadId, messages.Text?.Text,
                    replyParameters: source is null ? default(ReplyParameters?) : source);
                break;
            case MediaType.Photo:
                await _bot.SendPhotoAsync(id, file, threadId, messages.Text?.Text,
                    replyParameters: source is null ? default(ReplyParameters?) : source);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(messages));
        }
    }
}