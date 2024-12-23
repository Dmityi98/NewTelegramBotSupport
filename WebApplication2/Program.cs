using Microsoft.Extensions.Options;
using Telegram.Bot;
using WebApplication2;
using WebApplication2.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<ITelegramBotClient, TelegramBotClient>(ServiceProvider=>
{
    var token = ServiceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token;

    return new TelegramBotClient(token);
}
);
builder.Services.AddHostedService<TelegramBotBackgroundService>();

builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection(TelegramOptions.Telegram));
var host = builder.Build();

host.Run();

