using Bot.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Comands
{
    public class StartCommand : ICommandMessage
    {
        private readonly ITelegramBotClient _botClient;

        public StartCommand(ITelegramBotClient botClient)
        {
            _botClient = botClient;

        }
        public bool CanExecute(Message message)
        {
            return message.Text.Equals("/start", StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var textStart = "Приветствую вас в телеграмм боте от Academy TOP" +
                "\nЧтобы начать работу пришлите файл и выберите функцию по обратотке Exel файла\n" +
                "Или вы можете написать команду /help для получение более делальной информации по функциям";

            await _botClient.SendMessage(
                   chatId: message.Chat.Id,
                   text: textStart,
                   cancellationToken: cancellationToken);
        }
    }
}
