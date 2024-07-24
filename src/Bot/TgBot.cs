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
        config.Proxy.Enabled
            ? new(new HttpClientHandler
            {
                Proxy = new WebProxy(config.Proxy.Url, true, default,
                    config.Proxy.Credential.Enabled
                        ? new NetworkCredential(config.Proxy.Credential.UserName, config.Proxy.Credential.Password)
                        : default)
            })
            : default);

    private string? _userName;
    public string UserName => _userName ??= _bot.GetMeAsync().Result.Username!;

    public async Task<BotMessage> SendMessageAsync(MessagePair messages, BotMessage source, bool noReply)
    {
        return new(this,
            await SendMessageAsync(source.ChatId, messages, source.TgMessage?.MessageThreadId,
                noReply ? default : source.TgMessage));
    }

    public async Task SendMessageAsync(MessagePair messages, long id, TgMessage? msg)
    {
        await SendMessageAsync(id, messages, msg!.MessageThreadId, msg);
    }

    public async Task DeleteMessageAsync(BotMessage message)
    {
        await _bot.DeleteMessageAsync(message.TgMessage!.Chat.Id, message.TgMessage.MessageId);
    }

    public async Task<string> GetUserNameAsync(long userId, long chatId)
    {
        ChatMember userInfo = await _bot.GetChatMemberAsync(chatId, userId);
        return $"{userInfo.User.FirstName}{userInfo.User.LastName}";
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

    private async Task<TgMessage> SendMessageAsync(long id, MessagePair messages, int? threadId = null,
        TgMessage? source = null)
    {
        if (messages.Media is null)
        {
            return await _bot.SendTextMessageAsync(id, messages.Text!.Text, threadId,
                replyParameters: source is null ? default(ReplyParameters?) : source);
        }

        InputFile file = InputFile.FromStream(File.OpenRead(messages.Media.Path));
        return messages.Media.Type switch
        {
            MediaType.Audio => await _bot.SendAudioAsync(id, file, threadId, messages.Text?.Text,
                replyParameters: source is null ? default(ReplyParameters?) : source),
            MediaType.Photo => await _bot.SendPhotoAsync(id, file, threadId, messages.Text?.Text,
                replyParameters: source is null ? default(ReplyParameters?) : source),
            _ => throw new ArgumentOutOfRangeException(nameof(messages))
        };
    }
}