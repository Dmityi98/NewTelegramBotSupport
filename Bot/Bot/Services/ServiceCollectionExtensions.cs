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
using Bot.Database;
using Microsoft.EntityFrameworkCore;

namespace Bot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelegramBotServices(this IServiceCollection services)
        {
            services.AddHostedService<TelegramBotBackgroundService>();
            services.AddTransient<ITelegramBotClient>(ServiceProvider =>
            {
                var config = ServiceProvider.GetRequiredService<IConfiguration>();
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
            services.AddSingleton<AttendanceCallbackCommand>();
            services.AddSingleton<HomeworkCallbackCommand>();
            services.AddScoped<StudentHomeworkCallbackCommand>();
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
            services.AddScoped<ICallbackCommand, AttendanceCallbackCommand>();
            services.AddScoped<ICallbackCommand, HomeworkCallbackCommand>();
            services.AddScoped<ICallbackCommand, StudentHomeworkCallbackCommand>();
            return services;
        }
        public static IServiceCollection AddBotServices(this IServiceCollection services)
        {
            services.AddSingleton<FileStorageService>();
            services.AddScoped<WorkFileBuilder>();
            services.AddScoped<ReportBuilder>();
            services.AddSingleton<SendMessageTeacher>();
            return services;
        }
        public static IServiceCollection AddBotOptions(this IServiceCollection services, HostApplicationBuilder builder)
        {
            services.Configure<TelegramOptions>(builder.Configuration.GetSection(TelegramOptions.Telegram));
            builder.Services.AddScoped<DataService>();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Services.AddDbContext<DbContextBot>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}