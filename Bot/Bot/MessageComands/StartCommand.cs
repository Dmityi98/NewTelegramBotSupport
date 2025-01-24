using Bot.Database;
using Bot.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Comands
{
    public class StartCommand : ICommandMessage
    {
        private readonly ITelegramBotClient _botClient;
        private readonly DataService _dataService;
        private readonly ILogger<StartCommand> _logger;
        public StartCommand(ITelegramBotClient botClient, DataService dataService, ILogger<StartCommand> logger)
        {
            _botClient = botClient;
            _dataService = dataService; 
            _logger = logger;
        }
        public bool CanExecute(Message message)
        {
            return message.Text.Equals("/start", StringComparison.OrdinalIgnoreCase);
        }
        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var textStart = "Приветствую вас в телеграмм боте от Academy TOP" +
                "\nЧтобы начать работу пришлите файл и выберите функцию по обработке Exсel файла\n" +
                $"Или вы можете написать команду /help для получение более детальной информации по функциям";

            try
            {
                if (await _dataService.GetTeacherByChatIdAsync(message.Chat.Id) == null)
                {
                    string userName = message.Chat.FirstName ?? message.Chat.Username ?? "User";
                    await _dataService.AddTeacherAsync(userName, message.Chat.Id);
                    await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Hello {userName}, your data was saved {message.Chat.Id}", cancellationToken: cancellationToken);
                    _logger.LogInformation($"User {userName} with ChatId {message.Chat.Id} added to DB");
                }
                else
                {
                    await _botClient.SendMessage(chatId: message.Chat.Id, text: $"Hello, your data was already saved!", cancellationToken: cancellationToken);
                    _logger.LogInformation($"User with ChatId {message.Chat.Id} exist in DB");
                }
                await _botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: textStart,
                    cancellationToken: cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during command execution for chat ID {message.Chat.Id}");
                await _botClient.SendMessage(chatId: message.Chat.Id, text: $"An error occurred: {ex.Message}", cancellationToken: cancellationToken);
                return;
            }
        }
    }
}

