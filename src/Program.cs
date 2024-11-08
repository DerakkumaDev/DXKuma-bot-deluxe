using DXKumaBot;
using DXKumaBot.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tomlyn.Extensions.Configuration;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BotHostedService>();
builder.Configuration.Sources.Clear();
IHostEnvironment env = builder.Environment;
builder.Configuration.AddTomlFile("appsettings.toml", true, true);
builder.Configuration.AddTomlFile($"appsettings.{env.EnvironmentName}.toml", true, true);

builder.Configuration.Get<Config>();
using IHost host = builder.Build();
await host.RunAsync();