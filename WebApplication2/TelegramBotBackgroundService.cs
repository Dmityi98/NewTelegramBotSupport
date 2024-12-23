using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebApplication2.Options;

namespace WebApplication2
{
    public class TelegramBotBackgroundService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<TelegramBotBackgroundService> _logger;
        public TelegramBotBackgroundService(ILogger<TelegramBotBackgroundService> logger, IOptions<TelegramOptions> telegrtamOptions, ITelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
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
            // Некоторые действия
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            switch(messageText)
            {
                case "/start":
                    await SendStartMessage(chatId, cancellationToken);
                    break;
            }
           
        }

        private async Task SendStartMessage(long chatId,
            CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                [
                    InlineKeyboardButton.WithCallbackData(" проверенный работ", "date1"),
                     InlineKeyboardButton.WithCallbackData("Выданного дз", "date1")
                     ],
                new [] {
                    InlineKeyboardButton.WithCallbackData("Данные по уроку", "date1"),
                    InlineKeyboardButton.WithCallbackData("Посещаимость", "date1"),
                },
                 new [] {
                    InlineKeyboardButton.WithCallbackData("Отчет по студентам", "date1"),
                    InlineKeyboardButton.WithCallbackData("Выполнение дз", "date1")
                },
            });

            var textStart = "Привет, я умный помощник для работы деканата\nВыбери функцию для начала.";

            await _botClient.SendMessage(
                chatId:chatId,
                text: textStart,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
        Task HandErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n[{apiRequestException.Message}]",
                _ => exception.ToString()
            };
            return Task.CompletedTask;
        }

    }


}
