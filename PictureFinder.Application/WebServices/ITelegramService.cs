using System.Threading.Tasks;
using PictureFinder.Application.Dto;

namespace PictureFinder.Application.WebServices
{
    public interface ITelegramService
    {
        public Task SavePhotoWithTags(UpdateDto updateDto);
    }
}