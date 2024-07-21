namespace DXKumaBot;

public record Config(TelegramConfig Telegram);

public record TelegramConfig(string BotToken, ProxyConfig Proxy);

public record ProxyConfig(bool Enabled, string Url);