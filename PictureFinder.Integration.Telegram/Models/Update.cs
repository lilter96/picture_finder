namespace PictureFinder.Integration.Telegram.Models
{
    public class Update
    {
        public int UpdateId { get; set; }

        public Message Message { get; set; }

        public ChannelPost ChannelPost { get; set; }
    }
}