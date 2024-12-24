using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.Features
{
    public class CallbackHandler : IHendler<CallbackQuery>

    {
        private readonly ITelegramBotClient _botClient;
        public CallbackHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task Hendle(CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            switch (callback.Data)
            {
                case "lesson":
                    await _botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: "Нужно присать excel файл с данными о проверке домашенего задания",
                        cancellationToken: cancellationToken);
                    break;
            }
        }
    }
}
