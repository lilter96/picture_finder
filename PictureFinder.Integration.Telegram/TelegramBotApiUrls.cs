namespace PictureFinder.Integration.Telegram
{
    public static class TelegramBotApiUrls
    {
        public const string SendMessage = "sendMessage";

        public const string GetFileInfo = "getFile?file_id={0}";

        public const string DownloadFile = "{0}";
    }
}