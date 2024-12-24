using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Interface
{
    public interface ICommands
    {
        bool CanExecute(Message message);
        Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
    }
}
