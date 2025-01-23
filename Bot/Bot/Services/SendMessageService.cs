using Telegram.Bot;

namespace Bot.Services
{
    public class SendMessageService
    {
        private readonly TelegramBotClient _botClient;

        public SendMessageService(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMessage(string message, long chatId, CancellationToken cancellation)
        {
            await _botClient.SendMessage(
                chatId: chatId, 
                text: message, 
                cancellationToken: cancellation);
        }
    }
}
