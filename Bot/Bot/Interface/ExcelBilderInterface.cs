using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Interface
{
    public interface ExcelBilderInterface
    {
        void ReadFileExel();
        void Prosent();
        Task DownloadFileAsync(ITelegramBotClient botClient, Document document, CancellationToken cancellationToken);
    }
}
