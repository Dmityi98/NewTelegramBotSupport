namespace Bot.Services
{
    public class FileStorageService
    {
        private readonly Dictionary<long, string> _filePaths = new();
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(ILogger<FileStorageService> logger)
        {
            _logger = logger;
        }

        public void AddFilePath(long chatId, string filePath)
        {
            if (_filePaths.ContainsKey(chatId))
            {
                _logger.LogWarning("File path already exists for chatId: {chatId}, new path: {filePath}", chatId, filePath);
                _filePaths[chatId] = filePath;
                return;
            }
            _filePaths.Add(chatId, filePath);
            _logger.LogInformation("Added file path {filePath} for chatId: {chatId}", filePath, chatId);

        }

        public string GetFilePath(long chatId)
        {
            if (_filePaths.TryGetValue(chatId, out var filePath))
            {
                _logger.LogInformation("Get file path {filePath} for chatId: {chatId}", filePath, chatId);
                return filePath;
            }
            _logger.LogWarning("File path not found for chatId: {chatId}", chatId);
            return null;
        }

        public void ClearFilePath(long chatId)
        {
            if (_filePaths.Remove(chatId))
            {
                _logger.LogInformation("File path removed for chatId: {chatId}", chatId);
            }
            else
            {
                _logger.LogWarning("File path not found for chatId: {chatId}", chatId);
            }
        }
    }
}
