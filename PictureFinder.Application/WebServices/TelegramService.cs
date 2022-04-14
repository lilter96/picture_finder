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
        private readonly IPhotoRepository _photoRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly TelegramBotConfiguration _telegramBotConfiguration;

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

        public async Task SavePhotoWithTagsAsync(UpdateDto updateDto)
        {
            var photosContainer = GetObjectContainsPhotos(updateDto);
            var fileId = GetFileIdWithTheBestQuality(photosContainer.Photo);
            var mediaGroupId = photosContainer.MediaGroupId;

            if (string.IsNullOrEmpty(photosContainer.Caption))
                photosContainer.Caption = await GetTagsFromSameMediaGroupOrUtcAsync(mediaGroupId);

            var tags = ExtractTagsFromCaption(photosContainer.Caption);

            tags = await _tagRepository.SetIdsIfExists(tags);

            var filePath = await GetFilePathAsync(fileId);

            var photoUrl = CreatePhotoUrl(filePath);

            var addPhotoWithTagsDto = new AddPhotoWithTagsRequestDto
            {
                MediaGroupId = mediaGroupId,
                PhotoUrl = photoUrl,
                Tags = tags
            };

            await _photoRepository.AddPhotoWithTagsAsync(addPhotoWithTagsDto);
        }

        private static PhotosContainerDto GetObjectContainsPhotos(UpdateDto updateDto)
        {
            if (updateDto.Message != null &&
                updateDto.Message.Photo != null &&
                updateDto.Message.Photo.Count > 0)
                return updateDto.Message;

            if (updateDto.ChannelPost != null &&
                updateDto.ChannelPost.Photo != null &&
                updateDto.ChannelPost.Photo.Count > 0)
                return updateDto.ChannelPost;

            throw new UpdateObjectDoesNotContainPhotoException($"{nameof(updateDto)} does not contain photo.");
        }

        private static string GetFileIdWithTheBestQuality(IList<PhotoSizeDto> photos)
        {
            var maxSize = photos.Max(x => x.FileSize);
            var photo = photos.FirstOrDefault(x => x.FileSize == maxSize);

            return photo?.FileId;
        }

        private async Task<string> GetFilePathAsync(string fileId)
        {
            return await _telegramBotClient.GetFilePath(fileId);
        }

        private string CreatePhotoUrl(string filePath)
        {
            return _telegramBotConfiguration.BaseFilesApiUrl
                   + _telegramBotConfiguration.ApiKey
                   + "/"
                   + string.Format(TelegramBotApiUrls.DownloadFile, filePath);
        }

        private static List<Tag> ExtractTagsFromCaption(string caption)
        {
            return new List<Tag>
            {
                new Tag
                {
                    Name = caption
                }
            };
        }

        private async Task<string> GetTagsFromSameMediaGroupOrUtcAsync(string mediaGroupId)
        {
            var tags = await _photoRepository.GetTagsFromSameMediaGroupsAsync(mediaGroupId);

            if (tags == null || tags.Count == 0) return DateTime.UtcNow.ToLongDateString();

            return tags.First().Name;
        }
    }
}