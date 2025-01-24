using Bot.Interface;
using Bot.Logic.Builder;
using Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.CalbackCommand
{
    public class ReadFileCallbackCommand : ICallbackCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly WorkFileBuilder _reportBuilder;
        public  Dictionary<long, string> _filePaths = new();
        private readonly FileStorageService _fileStorage;

        public ReadFileCallbackCommand(ITelegramBotClient botClient, WorkFileBuilder workFileBuilder, FileStorageService fileStorage)
        {
            _botClient = botClient;
            _reportBuilder = workFileBuilder;
            _fileStorage = fileStorage;
        }
        public bool CanExecute(CallbackQuery callback)
        {
            return callback.Data.Equals("read", StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            InlineKeyboardMarkup inlineKeyboard = new(
                InlineKeyboardButton.WithCallbackData("За месяц", "mounth"),
                InlineKeyboardButton.WithCallbackData("За неделю", "week")
                
            );
            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Выберите кнопку",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
