using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using SupportBot.Features;
using SupportBot.Options;

namespace SupportBot
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger, IOptions<TelegramOptions> telegrtamOptions, ITelegramBotClient botClient, IServiceScopeFactory serviceScope)
        {
            _logger = logger;
            _botClient = botClient;
            _serviceScope = serviceScope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = []
            };


            while (!stoppingToken.IsCancellationRequested)
            {
                await _botClient.ReceiveAsync(
                    updateHandler: HandleUpdateAsync,
                    errorHandler: HandErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: stoppingToken);
            }

        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var scope = _serviceScope.CreateScope();

            var messageHender = scope.ServiceProvider.GetRequiredService<IHendler<Message>>();

            var hendler = update switch
            {
                { Message: { } message } => messageHender.Hendle(message, cancellationToken),
                { CallbackQuery: { } callback } => CallbackQueryHendler(callback, cancellationToken),
                _ => UnknownUpdateHendlerAsync(update, cancellationToken)
            };

            await hendler;

        }

        private Task UnknownUpdateHendlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown message");
            return Task.CompletedTask;
        }

        private async Task CallbackQueryHendler(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Message is not { } message)
                return;

            switch (callbackQuery.Data)
            {
                case "lesson":
                    await _botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: "Приветик, пришли файл эксель",
                        cancellationToken: cancellationToken);
                    break;
            }
        }
        Task HandErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            switch (exception)
            {
                case (ApiRequestException apiRequestException):
                    _logger.LogError(
                        apiRequestException,
                        "Telegram API Error:\n{errorCode}\n{Meccage}",
                        apiRequestException.ErrorCode,
                        apiRequestException.Message);
                    return Task.CompletedTask;

                default:
                    _logger.LogError(exception, "Error while processing message in telegram bot");
                    return Task.CompletedTask;
            };
        }
    }
}
