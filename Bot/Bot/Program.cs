using Bot.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTelegramBotServices()
    .AddBotHandlers()
    .AddBotCommands()
    .AddBotCallbackCommands()
    .AddBotServices()
    .AddBotOptions(builder);
SQLitePCL.Batteries.Init();
var host = builder.Build();
host.Run();