using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PictureFinder.Application.Dto;
using PictureFinder.Application.Exceptions;
using PictureFinder.Data.Repositories;
using PictureFinder.Domain.Photo.Dto;
using PictureFinder.Domain.Tag;
using PictureFinder.Integration.Telegram;

namespace PictureFinder.Application.WebServices
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly TelegramBotConfiguration _telegramBotConfiguration;
        private readonly IPhotoRepository _photoRepository;
        private readonly ITagRepository _tagRepository;

        public TelegramService(
            ITelegramBotClient telegramBotClient,
            TelegramBotConfiguration telegramBotConfiguration, 
            IPhotoRepository photoRepository, 
            ITagRepository tagRepository)
        {
            _telegramBotClient = telegramBotClient;
            _telegramBotConfiguration = telegramBotConfiguration;
            _photoRepository = photoRepository;
            _tagRepository = tagRepository;
        }

        public async Task SavePhotoWithTags(UpdateDto updateDto)
        {
            var photosContainer = GetObjectContainsPhotos(updateDto);
            var fileId = GetFileIdWithTheBestQuality(photosContainer.Photo);
            var tags = ExtractTagsFromCaption(photosContainer.Caption, photosContainer.CaptionEntities);

            tags = await _tagRepository.SetIdsIfExists(tags);

            if (tags.Count == 0)
            {
                tags.Add(new Tag
                {
                    Name = DateTime.UtcNow.ToShortTimeString()
                });
            }
            
            var filePath = await GetFilePath(fileId);

            var photoUrl = CreatePhotoUrl(filePath);

            var addPhotoWithTagsDto = new AddPhotoWithTagsRequestDto
            {
                PhotoUrl = photoUrl,
                Tags = tags
            };

            var photo = await _photoRepository.AddPhotoWithTags(addPhotoWithTagsDto);
        }

        private static PhotosContainerDto GetObjectContainsPhotos(UpdateDto updateDto)
        {
            if (updateDto.Message != null && 
                updateDto.Message.Photo != null && 
                updateDto.Message.Photo.Count > 0)
            {
                return updateDto.Message;
            }

            if (updateDto.ChannelPost != null && 
                updateDto.ChannelPost.Photo != null &&
                updateDto.ChannelPost.Photo.Count > 0)
            {
                return updateDto.ChannelPost;
            }

            throw new UpdateObjectDoesNotContainPhotoException($"{nameof(updateDto)} does not contain photo.");
        }

        private string GetFileIdWithTheBestQuality(IList<PhotoSizeDto> photos)
        {
            var maxSize = photos.Max(x => x.FileSize);
            var photo = photos.FirstOrDefault(x => x.FileSize == maxSize);

            return photo.FileId;
        }

        private async Task<string> GetFilePath(string fileId)
        {
            return await _telegramBotClient.GetFilePath(fileId);
        }

        private string CreatePhotoUrl(string filePath) =>
            _telegramBotConfiguration.BaseBotApiUrl
            + _telegramBotConfiguration.ApiKey
            + "/"
            + string.Format(TelegramBotApiUrls.DownloadFile, filePath);

        private static List<Tag> ExtractTagsFromCaption(string caption, List<MessageEntityDto> captionEntities)
        {
            var tags = new List<Tag>();

            captionEntities.ForEach(captionEntity =>
            {
                var tagText = new string(caption.Skip(captionEntity.Offset).Take(captionEntity.Length).ToArray());;
                tags.Add(new Tag
                {
                    Name = tagText
                });
            });

            return tags;
        }
    }
}