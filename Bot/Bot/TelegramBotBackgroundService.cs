using Bot.CalbackCommand;
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
        private static string _downloadPath = "downloaded_files";
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        private readonly CommandMessageHandler _commandMessageHandler;
        private readonly CommandCallbackHandler _commandCallbackHandler;

        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger,
                                            IOptions<TelegramOptions> telegrtamOptions,
                                            ITelegramBotClient botClient,
                                            IServiceScopeFactory serviceScope,
                                            CommandMessageHandler commandHandler,
                                            CommandCallbackHandler commandCallbackHandler)
        {
            _logger = logger;
            _botClient = botClient;
            _serviceScope = serviceScope;
            _commandMessageHandler = commandHandler;
            _commandCallbackHandler = commandCallbackHandler;
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

            var hendler = update switch
            {
                { Message: { Document: { } document } message } => DownloadFileAsync(botClient, message, cancellationToken), // Исправлено
                { Message: { } message } => _commandMessageHandler.HandleAsync(botClient, message, cancellationToken),
                { CallbackQuery: { } callback } => _commandCallbackHandler.HandleAsync(botClient,callback, cancellationToken),
                _ => UnknownUpdateHendlerAsync(update, cancellationToken)
            };

            await hendler;

        }

        private async Task DownloadFileAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.Document is not { } document) return;
            var chatId = message.Chat.Id;

            try
            {
                var fileInfo = await botClient.GetFileAsync(document.FileId, cancellationToken); 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), _downloadPath, document.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await botClient.DownloadFile(fileInfo.FilePath, fileStream, cancellationToken); 
                }

                await botClient.SendMessage(chatId,
                    text: "Файл успешно скачен", cancellationToken: cancellationToken); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when download file");
                await botClient.SendMessage(chatId, $"Ошибка при скачивании файла: {ex.Message}", cancellationToken: cancellationToken); // Исправлено
            }
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
