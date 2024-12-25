using Bot.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CalbackCommand
{
    public class ReadFileCallbackCommand : ICallbackCommand
    {
        private readonly ITelegramBotClient _botClient;

        public ReadFileCallbackCommand(ITelegramBotClient botClient)
        {
            _botClient = botClient;

        }
        public bool CanExecute(CallbackQuery callback)
        {
            return callback.Data.Equals("read", StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Нужно присать excel файл с данными о проверке домашенего задания",
                cancellationToken: cancellationToken);
        
        }
    }
}
