using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using SupportBot;
using SupportBot.Features;
using SupportBot.Options;
using Bot.Features;
using Bot.Interface;
using Bot.Options;
using Bot.Comands;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<TelegramBotBackgroundService>();

builder.Services.AddTransient<ITelegramBotClient, TelegramBotClient>(ServiceProvider =>
{
    var token = ServiceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token;

    return new TelegramBotClient(token);
}
);
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddSingleton<ICommands, StartCommand>();
builder.Services.AddSingleton<ICommands, HelpCommand>();
builder.Services.AddTransient<IHendler<CallbackQuery>, CallbackHandler>();

builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection(TelegramOptions.Telegram));

var host = builder.Build();

host.Run();

