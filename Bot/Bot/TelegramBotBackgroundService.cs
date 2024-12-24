using Bot.Comands;
using Bot.Features;
using Bot.Interface;
using Microsoft.Extensions.Options;
using SupportBot.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SupportBot
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        private readonly CommandHandler _commandHandler;
        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger,
                                            IOptions<TelegramOptions> telegrtamOptions,
                                            ITelegramBotClient botClient,
                                            IServiceScopeFactory serviceScope, CommandHandler commandHandler)
        {
            _logger = logger;
            _botClient = botClient;
            _serviceScope = serviceScope;
            _commandHandler = commandHandler;
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
            var callbackHandler = scope.ServiceProvider.GetRequiredService<IHendler<CallbackQuery>>();

            var hendler = update switch
            {
                { Message: { } message } => _commandHandler.HandleAsync(botClient, message, cancellationToken),
                { CallbackQuery: { } callback } => callbackHandler.Hendle(callback, cancellationToken),
                _ => UnknownUpdateHendlerAsync(update, cancellationToken)
            };

            await hendler;

        }

        private Task UnknownUpdateHendlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown message");
            return Task.CompletedTask;
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
