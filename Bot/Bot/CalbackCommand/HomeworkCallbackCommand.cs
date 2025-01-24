using Bot.Interface;
using Bot.Logic.Builder;
using Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CalbackCommand
{
    public class HomeworkCallbackCommand : ICallbackCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly WorkFileBuilder _workFileBuilder;
        private readonly FileStorageService _fileStorageService;
        private Dictionary<long, string> _filePath = new();

        public HomeworkCallbackCommand(ITelegramBotClient botClient,
                                       WorkFileBuilder workFileBuilder, 
                                       FileStorageService fileStorageService)
        {
            _botClient = botClient;
            _workFileBuilder = workFileBuilder;
            _fileStorageService = fileStorageService;
        }
        public bool CanExecute(CallbackQuery callback)
        {
            return callback.Data.Equals("homework", StringComparison.OrdinalIgnoreCase);
        }

        async public Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Идёт обработка файла и формирование отчета...",
                cancellationToken: cancellationToken);
            var filePath = _fileStorageService.GetFilePath(message.Chat.Id);

            if (filePath is not null)
            {
                var report= _workFileBuilder.ReportErrorStudendAverage(filePath);
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: $"Дз у студентов процент выполнения дз меньше 50% \n{report}\n",
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

