namespace PictureFinder.Integration.Telegram.Models
{
    public class SendMessageRequest
    {
        public string ChatId { get; set; }

        public string Text { get; set; }

        public int? ReplyToMessageId { get; set; }
    }
}