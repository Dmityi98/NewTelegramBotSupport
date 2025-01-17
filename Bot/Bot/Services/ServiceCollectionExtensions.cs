using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

namespace Bot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
        {
            services.AddHostedService<TelegramBotBackgroundService>();
            services.AddTransient<ITelegramBotClient>(ServiceProvider =>
            {
                var token = ServiceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value.Token;
                return new TelegramBotClient(token);
            });
            return services;
        }
        public static IServiceCollection AddBotHandlers(this IServiceCollection services)
        {
            services.AddSingleton<CommandMessageHandler>();
            services.AddSingleton<CommandCallbackHandler>();
            services.AddSingleton<IssuedCallbackCommand>();
            return services;
        }
        public static IServiceCollection AddBotCommands(this IServiceCollection services)
        {
            services.AddSingleton<ICommandMessage, StartCommand>();
            services.AddSingleton<ICommandMessage, HelpCommand>();
            return services;
        }
        public static IServiceCollection AddBotCallbackCommands(this IServiceCollection services)
        {
            services.AddScoped<ICallbackCommand, ReadFileCallbackCommand>();
            services.AddScoped<ICallbackCommand, WeekCallbackCommand>();
            services.AddScoped<ICallbackCommand, MonthCallbackCommand>();
            services.AddScoped<ICallbackCommand, TopicCallbackCommand>();
            services.AddScoped<ICallbackCommand, IssuedCallbackCommand>();
            return services;
        }
        public static IServiceCollection AddBotServices(this IServiceCollection services)
        {
            services.AddSingleton<FileStorageService>();
            services.AddScoped<WorkFileBuilder>();
            return services;
        }
        public static IServiceCollection AddBotOptions(this IServiceCollection services, HostApplicationBuilder builder)
        {
            services.Configure<TelegramOptions>(builder.Configuration.GetSection(TelegramOptions.Telegram));
            return services;
        }
    }
}