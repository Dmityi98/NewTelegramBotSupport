using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Interface
{
    public interface ICommandMessage
    {
        bool CanExecute(Message message);
        Task ExecuteAsync( Message message, CancellationToken cancellationToken);
    }
}
