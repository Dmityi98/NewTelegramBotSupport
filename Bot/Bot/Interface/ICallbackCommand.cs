using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Interface
{
    public interface ICallbackCommand
    {
        bool CanExecute(CallbackQuery callback);
        Task ExecuteAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken);
    }
}
