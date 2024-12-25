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

            var textStart = "Это сообщение отправлено на команду /help";

            await _botClient.SendMessage(
                chatId: message.Chat.Id,
                text: textStart,
                cancellationToken: cancellationToken);
        }
    }
}
