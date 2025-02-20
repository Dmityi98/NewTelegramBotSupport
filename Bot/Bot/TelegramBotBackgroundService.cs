using Bot.Database;
using Bot.Features;
using Bot.Services;
using Microsoft.Extensions.Options;
using SupportBot.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SupportBot
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly DataService _teachersService;
        private static string _downloadPath = "downloaded_files";
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        private readonly CommandMessageHandler _commandMessageHandler;
        private readonly CommandCallbackHandler _commandCallbackHandler;
        private readonly FileStorageService _fileStorage;
        public Dictionary<long, string> _filePaths = new();

        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger,
                                            IOptions<TelegramOptions> telegrtamOptions,
                                            ITelegramBotClient botClient,
                                            CommandMessageHandler commandHandler,
                                            CommandCallbackHandler commandCallbackHandler,
                                            FileStorageService fileStorageService,
                                            DataService teachersService)
        {
            _logger = logger;
            _botClient = botClient;
            _commandMessageHandler = commandHandler;
            _commandCallbackHandler = commandCallbackHandler;
            _fileStorage = fileStorageService;
            _teachersService = teachersService;
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
                { Message: { } message } when message.Document != null => HandleDocumentAsync(botClient, message, cancellationToken),
                { Message: { } message } => _commandMessageHandler.HandleAsync(botClient, message, cancellationToken),
                { CallbackQuery: { } callback } => _commandCallbackHandler.HandleAsync(botClient,callback, cancellationToken),
                _ => UnknownUpdateHendlerAsync(update, cancellationToken)
            };
            await hendler;
        }
        public async Task HandleDocumentAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(_downloadPath);
            if (message.Document is null)
                return;
            
            var chatId = message.Chat.Id;

            var fileInfo = await botClient.GetFile(message.Document.FileId, cancellationToken);
            var filePath = Path.Combine(_downloadPath, message.Document.FileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await botClient.DownloadFile(fileInfo.FilePath, fileStream, cancellationToken);
            }
            await botClient.SendMessage(message.Chat.Id,
                  text: "���� ������� ������", cancellationToken: cancellationToken);

            _fileStorage.AddFilePath(chatId, filePath);
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                [
                    InlineKeyboardButton.WithCallbackData("������ � ����������� �����", "read"),
                    InlineKeyboardButton.WithCallbackData("������ �� �����", "topic")
                     ],
                new [] {
                    InlineKeyboardButton.WithCallbackData("��������� ��", "issued"),
                    InlineKeyboardButton.WithCallbackData("������������", "attendance"),
                },
                 new [] {
                    InlineKeyboardButton.WithCallbackData("����� �� ���������", "homework"),
                    InlineKeyboardButton.WithCallbackData("���������� �� ����������", "studenthomework")
                },
            });
            var textStart = "������, � ����� �������� ��� ������ ��������\n������ ������� ��� ������.";

            await botClient.SendMessage(message.Chat.Id,
                  text: textStart,
                  cancellationToken: cancellationToken,
                  replyMarkup: inlineKeyboard);
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
