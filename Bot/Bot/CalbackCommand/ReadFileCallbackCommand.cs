using Bot.Core.Models;
using Bot.Interface;
using Bot.Logic.Builder;
using Bot.Services;
using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CalbackCommand
{
    public class ReadFileCallbackCommand : ICallbackCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ReportBuilder _reportBuilder = new ReportBuilder();
        public  Dictionary<long, string> _filePaths = new();
        private readonly FileStorageService _fileStorage;



        public ReadFileCallbackCommand(ITelegramBotClient botClient, ReportBuilder reportBuilder, FileStorageService fileStorage)
        {
            _botClient = botClient;
            _reportBuilder = reportBuilder;
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

            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Идёт обработка файла и формирование отчета...",
                cancellationToken: cancellationToken);
            var filePath = _fileStorage.GetFilePath(message.Chat.Id);
            
            if (filePath is not null)
            {
                _reportBuilder.FileExcelRead(filePath);
                var tlist = _reportBuilder.GetTeacherList();
                string NameTeachersStr = string.Join("\n", tlist.Select(n => n.NameTeacher));

                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: NameTeachersStr,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: "Файл не найден.",
                    cancellationToken: cancellationToken);
            }
        }
    }
}
