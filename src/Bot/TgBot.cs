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

    private string? _userName;
    public string UserName => _userName ??= _bot.GetMeAsync().Result.Username!;

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
#if DEBUG
        _bot.OnError += (e, _) =>
        {
            Console.WriteLine(e);
            return Task.CompletedTask;
        };
#endif
        _bot.OnMessage += async (message, type) =>
        {
            if (type is not UpdateType.Message)
            {
                return;
            }

            switch (message)
            {
                case { Type: MessageType.Text, Text: not null } when MessageReceived is not null:
                    await MessageReceived.Invoke(_bot, new(this, message));
                    return;
                case { Type: MessageType.ChatMembersAdded, NewChatMembers: not null } when MembersAdded is not null:
                    await MembersAdded.Invoke(_bot, new(this, message));
                    return;
                case { Type: MessageType.ChatMemberLeft, LeftChatMember: not null } when MembersLeft is not null:
                    await MembersLeft.Invoke(_bot, new(this, message));
                    break;
            }
        };
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