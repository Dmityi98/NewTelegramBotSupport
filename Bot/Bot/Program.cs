
using Bot.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTelegramBotServices()
    .AddBotHandlers()
    .AddBotCommands()
    .AddBotCallbackCommands()
    .AddBotServices()
    .AddBotOptions(builder);


var host = builder.Build();

host.Run();