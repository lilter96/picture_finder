using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PictureFinder.Integration.Telegram.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace PictureFinder.Integration.Telegram
{
    public class TelegramBotClient : ITelegramBotClient
    {
        private readonly RestClient _botApiClient;
        private readonly RestClient _downloadFilesApiClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public TelegramBotClient(
            TelegramBotConfiguration telegramBotConfiguration,
            JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;

            _botApiClient = new RestClient(telegramBotConfiguration.BaseBotApiUrl + telegramBotConfiguration.ApiKey);
            _botApiClient.UseNewtonsoftJson(_jsonSerializerSettings);

            _downloadFilesApiClient =
                new RestClient(telegramBotConfiguration.BaseFilesApiUrl + telegramBotConfiguration.ApiKey);
            _downloadFilesApiClient.UseNewtonsoftJson(_jsonSerializerSettings);
        }

        public async Task SendMessage(SendMessageRequest message)
        {
            var request = new RestRequest(TelegramBotApiUrls.SendMessage)
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(message);

            var response = await _botApiClient.ExecuteAsync(request);

            if (!response.IsSuccessful) throw new ApplicationException();
        }

        public async Task<DownloadPhotoResponse> DownloadPhoto(DownloadPhotoRequest downloadPhotoRequest)
        {
            var filePath = await GetFilePath(downloadPhotoRequest.FileId);

            var url = string.Format(TelegramBotApiUrls.DownloadFile, filePath);
            var request = new RestRequest(url)
            {
                Method = Method.Get
            };

            var response = await _downloadFilesApiClient.DownloadDataAsync(request);

            var path = Path.Combine("D:\\", DateTime.UtcNow.Second + ".jpg");
            await File.WriteAllBytesAsync(path, response);

            return new DownloadPhotoResponse();
        }

        public async Task<string> GetFilePath(string fileId)
        {
            var url = string.Format(TelegramBotApiUrls.GetFileInfo, fileId);
            var request = new RestRequest(url)
            {
                Method = Method.Get
            };


            var response = await _botApiClient.ExecuteAsync(request);

            var fileInfoResponse =
                JsonConvert.DeserializeObject<GetFileInfoResponse>(response.Content, _jsonSerializerSettings);

            return fileInfoResponse.Result.FilePath;
        }
    }
}