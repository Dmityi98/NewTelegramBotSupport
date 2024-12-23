using Bot.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SupportBot.Features
{
    public class MessageHendler : IHendler<Message>

    {
        private readonly ITelegramBotClient _botClient;

        public MessageHendler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
           
        }

        public async Task Hendle(Message message, CancellationToken cancellationToken)
        {
            switch (message.Text)
            {
                case "/start":
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                [
                    InlineKeyboardButton.WithCallbackData("проверенный работ", "lesson"),
                    InlineKeyboardButton.WithCallbackData("Выданного дз", "date2")
                     ],
                new [] {
                    InlineKeyboardButton.WithCallbackData("Данные по уроку", "date3"),
                    InlineKeyboardButton.WithCallbackData("Посещаимость", "date4"),
                },
                 new [] {
                    InlineKeyboardButton.WithCallbackData("Отчет по студентам", "date5"),
                    InlineKeyboardButton.WithCallbackData("Выполнение дз", "date6")
                },
            });

                    var textStart = "Привет, я умный помощник для работы деканата\nВыбери функцию для начала.";

                    await _botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: textStart,
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                    break;
            }

            
        }
    }

}
