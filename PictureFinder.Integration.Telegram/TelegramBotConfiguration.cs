using EnsureThat;

namespace PictureFinder.Integration.Telegram
{
    public class TelegramBotConfiguration
    {
        public TelegramBotConfiguration(
            string baseBotApiUrl,
            string baseFilesApiUrl,
            string apiKey)
        {
            BaseBotApiUrl = Ensure.String.IsNotNullOrEmpty(baseBotApiUrl, nameof(baseBotApiUrl));
            BaseFilesApiUrl = Ensure.String.IsNotNullOrEmpty(baseFilesApiUrl, nameof(baseFilesApiUrl));
            ApiKey = Ensure.String.IsNotNullOrEmpty(apiKey, nameof(apiKey));
        }


        public string BaseBotApiUrl { get; set; }

        public string BaseFilesApiUrl { get; set; }

        public string ApiKey { get; set; }
    }
}