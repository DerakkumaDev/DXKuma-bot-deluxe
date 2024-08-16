namespace DXKumaBot;

public sealed record Config(TelegramConfig Telegram);

public sealed record TelegramConfig(string BotToken, ProxyConfig Proxy);

public sealed record ProxyConfig(bool Enabled, Uri Url, ProxyCredentialConfig Credential);

public sealed record ProxyCredentialConfig(bool Enabled, string UserName, string Password);