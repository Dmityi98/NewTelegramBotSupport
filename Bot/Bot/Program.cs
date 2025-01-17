using Bot.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTelegramBotServices()
    .AddBotHandlers()
    .AddBotCommands()
    .AddBotCallbackCommands()
    .AddBotServices()
    .AddBotOptions(builder);
var host = builder.Build();
host.Run();