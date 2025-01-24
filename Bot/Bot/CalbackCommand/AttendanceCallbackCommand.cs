﻿using Bot.Interface;
using Bot.Logic.Builder;
using Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CalbackCommand
{
    public class AttendanceCallbackCommand : ICallbackCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly WorkFileBuilder _reportBuilder;
        public Dictionary<long, string> _filePaths = new();
        private readonly FileStorageService _fileStorage;
        private readonly SendMessageTeacher _sendMessageTeacher;

        public AttendanceCallbackCommand(ITelegramBotClient botClient, WorkFileBuilder workFileBuilder, FileStorageService fileStorage, SendMessageTeacher sendMessageTeacher)
        {
            _botClient = botClient;
            _reportBuilder = workFileBuilder;
            _fileStorage = fileStorage;
            _sendMessageTeacher = sendMessageTeacher;
        }
        public bool CanExecute(CallbackQuery callback)
        {
            return callback.Data.Equals("attendance", StringComparison.OrdinalIgnoreCase);
        }
        async public Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
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
                var report = _reportBuilder.ReportAttendence(filePath);
                await _sendMessageTeacher.SendMessageTeachers("у вас процент посещаимости ниже 66%");
                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: $"У данных преподователей процент посещаимости ниже 66% \n{report}\n",
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
