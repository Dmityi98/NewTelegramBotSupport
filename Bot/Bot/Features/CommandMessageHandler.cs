using System.Windows.Input;
using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.Features
{
    public class CommandMessageHandler
    {
        private readonly IEnumerable<ICommandMessage> _commands;

        public CommandMessageHandler(IEnumerable<ICommandMessage> commands)
        {
            _commands = commands;
        }

        public async Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            foreach (var command in _commands)
            {
                if (command.CanExecute(message))
                {
                    await command.ExecuteAsync(message, cancellationToken);
                    return;
                }
            }

            await botClient.SendMessage(message.Chat.Id, "Неизвестная команда.", cancellationToken: cancellationToken);
        }
    }
}
