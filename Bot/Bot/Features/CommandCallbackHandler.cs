using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.Features
{
    public class CommandCallbackHandler
    {
        private readonly IEnumerable<ICallbackCommand> _commands;

        public CommandCallbackHandler(IEnumerable<ICallbackCommand> commands)
        {
            _commands = commands;
        }

        public async Task HandleAsync(ITelegramBotClient botClient, CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;
            foreach (var command in _commands)
            {
                if (command.CanExecute(callback))
                {
                    await command.ExecuteAsync(botClient, callback, cancellationToken);
                    return;
                }
            }

            await botClient.SendMessage(message.Chat.Id, "Неизвестная callback.", cancellationToken: cancellationToken);
        }
    }
}
