namespace PictureFinder.Application.Dto
{
    public class UpdateDto
    {
        public int UpdateId { get; set; }

        public MessageDto Message { get; set; }

        public ChannelPostDto ChannelPost { get; set; }
    }
}