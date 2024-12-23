using Telegram.Bot.Types;
using Telegram.Bot;
using Bot.Interface;

namespace Bot.Features
{
    public class CallbackHandler : IHendler<CallbackQuery>

    {
        private readonly ITelegramBotClient _botClient;
        private readonly IService _service;
        public CallbackHandler(ITelegramBotClient botClient, IService service)
        {
            _botClient = botClient;
            _service = service;
        }

        public async Task Hendle(CallbackQuery callback, CancellationToken cancellationToken)
        {
            if (callback.Message is not { } message)
                return;

            switch (callback.Data)
            {
                case "lesson":
                    _service.ReadDocumentExcel(""); // заменить на filePath
                    await _botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: "Нужно присать excel файл с данными о проверке домашенего задания",
                        cancellationToken: cancellationToken);
                    break;
            }
        }
    }
}
