using Bot.Logic.Builder;
using Bot.Services;
using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.CalbackCommand
{
    public class MonthCallbackCommand : ICallbackCommand
    {

        private readonly ITelegramBotClient _botClient;
        private readonly WorkFileBuilder _reportBuilder;
        public Dictionary<long, string> _filePaths = new();
        private readonly FileStorageService _fileStorage;

        public MonthCallbackCommand(ITelegramBotClient botClient, WorkFileBuilder workFileBuilder, FileStorageService fileStorage)
        {
            _botClient = botClient;
            _reportBuilder = workFileBuilder;
            _fileStorage = fileStorage;
        }
        public bool CanExecute(CallbackQuery callback)
        {
             return  callback.Data.Equals("mounth", StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Идёт обработка файла и формирование отчета...",
                cancellationToken: cancellationToken);
            var filePath = _fileStorage.GetFilePath(message.Chat.Id);

            if (filePath is not null)
            {
                var report = _reportBuilder.ReportErrorMonth(filePath);

                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: $"У данных преподователей проверка дз меньше 75% за этот месяц\n{report}\n",
                    cancellationToken: cancellationToken);
                
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: $"Пришлите файл ещё раз файл для начала новой работы",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: "Файл не найден.",
                    cancellationToken: cancellationToken);
            }
            System.IO.File.Delete(filePath);
        }
    }
}
