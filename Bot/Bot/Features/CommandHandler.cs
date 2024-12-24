using System.Windows.Input;
using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.Features
{
    public class CommandHandler
    {
        private readonly IEnumerable<ICommands> _commands;

        public CommandHandler(IEnumerable<ICommands> commands)
        {
            _commands = commands;
        }

        public async Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            foreach (var command in _commands)
            {
                if (command.CanExecute(message))
                {
                    await command.ExecuteAsync(botClient, message, cancellationToken);
                    return;
                }
            }

            await botClient.SendMessage(message.Chat.Id, "Неизвестная команда.", cancellationToken: cancellationToken);
        }
    }
}
