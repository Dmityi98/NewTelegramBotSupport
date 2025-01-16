using Bot.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Comands
{
    public class HelpCommand : ICommandMessage
    {
        private readonly ITelegramBotClient _botClient;

        public HelpCommand(ITelegramBotClient botClient)
        {
            _botClient = botClient;

        }
        public bool CanExecute(Message message)
        {
            return message.Text.Equals("/help", StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {

            var textHelp = "Для начала работы вам нужно прислать файл Excel:\n" +
                "Действия:\n1) Когда вы пришлёте файл Excel вам предстоит выбрать функцию для работы с файлом" +
                "(тут ошибаться нельзя, иначе вы получите неправильный отчёт)\n" +
                "2)Дождаться обработки файла и получить правильную информацию от бота\nХорошего дня!";

            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: textHelp,
                cancellationToken: cancellationToken);
        }
    }
}
