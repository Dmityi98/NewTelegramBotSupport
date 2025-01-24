using Bot.Database;
using Bot.Logic.Builder;
using Microsoft.EntityFrameworkCore.Internal;
using Telegram.Bot;

namespace Bot.Services
{
    public class SendMessageTeacher
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ReportBuilder _reportBuilder;
        private readonly DataService _contextServices;

        public SendMessageTeacher(ReportBuilder reportBuilder, DataService contextServices, ITelegramBotClient telegramBot)
        {
            _reportBuilder = reportBuilder;
            _contextServices = contextServices;
            _botClient = telegramBot;
        }

        public async Task SendMessageTeachers(string text)
        {
            Console.WriteLine("Функция отправки сообщений");
            Console.WriteLine($"Кол-во преподавателей {_reportBuilder.PercentageOfHomeworkCompletedWeek().Count}");
            foreach (var item in _reportBuilder.PercentageOfHomeworkCompletedWeek())
            {
                var teacherName = item.NameTeacher; // Получаем имя преподавателя из отчета
                var teacherChatId = await _contextServices.GetChatId(teacherName);

                Console.WriteLine($"Report teacher name {teacherName}");
                if (teacherChatId.HasValue)
                {
                    await _botClient.SendMessage(chatId: teacherChatId.Value, text: $"{teacherName} {text}");
                    Console.WriteLine("Сообщение было отправлено");
                }
                else
                {
                    Console.WriteLine($"ChatId not found for teacher {teacherName}");
                }
            }
        }
    }
}
