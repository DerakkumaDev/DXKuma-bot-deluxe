namespace DXKumaBot;

public sealed record Config(TelegramConfig Telegram);

public sealed record TelegramConfig(string BotToken, ProxyConfig Proxy);

public sealed record ProxyConfig(bool Enabled, string Url);