namespace PictureFinder.Application.Dto
{
    public class PhotoSizeDto
    {
        public string FileId { get; set; }

        public string FileUniqueId { get; set; }

        public int FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}