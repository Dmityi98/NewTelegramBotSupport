using Microsoft.Extensions.Options;
using Telegram.Bot;
using SupportBot;
using SupportBot.Options;
using Bot.Features;
using Bot.Interface;
using Bot.Comands;
using Bot.CalbackCommand;
using Bot.Logic.Builder;
using Bot.Services;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<TelegramBotBackgroundService>();

builder.Services.AddTransient<ITelegramBotClient, TelegramBotClient>(ServiceProvider =>
{
    var token = ServiceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token;

    return new TelegramBotClient(token);
}
);
builder.Services.AddSingleton<CommandMessageHandler>();
builder.Services.AddSingleton<CommandCallbackHandler>();
builder.Services.AddSingleton<ICommandMessage, StartCommand>();
builder.Services.AddSingleton<ICommandMessage, HelpCommand>();
builder.Services.AddSingleton<ICallbackCommand, ReadFileCallbackCommand>();
builder.Services.AddSingleton<ICallbackCommand, WeekCallbackCommand>();
builder.Services.AddSingleton<ICallbackCommand, MonthCallbackCommand>();
builder.Services.AddSingleton<FileStorageService>();

builder.Services.AddScoped<Bot.Logic.Builder.WorkFileBuilder>();

builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection(TelegramOptions.Telegram));

var host = builder.Build();

host.Run();

