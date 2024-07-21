using DXKumaBot;
using DXKumaBot.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Tomlyn.Extensions.Configuration;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
IHostEnvironment env = builder.Environment;
builder.Configuration.AddTomlFile("appsettings.toml", true, true)
    .AddTomlFile($"appsettings.{env.EnvironmentName}.toml", true, true);

Config config = builder.Configuration.Get<Config>()!;
BotInstance bot = new(config);
await bot.RunAsync();