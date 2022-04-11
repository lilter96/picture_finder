namespace PictureFinder.Integration.Telegram.Models
{
    public class GetFileInfoResult
    {
        public string FileId { get; set; }

        public string FileSize { get; set; }

        public string FilePath { get; set; }
    }
}