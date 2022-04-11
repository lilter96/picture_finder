namespace PictureFinder.Integration.Telegram.Models
{
    public class PhotoSize
    {
        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public int FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}