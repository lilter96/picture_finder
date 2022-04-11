using System.Threading.Tasks;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Integration.Telegram
{
    public interface ITelegramBotClient
    {
        public Task SendMessage(SendMessageRequest message);

        public Task<DownloadPhotoResponse> DownloadPhoto(DownloadPhotoRequest downloadPhotoRequest);

        public Task<string> GetFilePath(string fileId);
    }
}