using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PictureFinder.Application.Dto;
using PictureFinder.Application.WebServices;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.Controller
{
    [ApiController]
    [Route("")]
    public class TelegramController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITelegramService _telegramService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public TelegramController(
            JsonSerializerSettings jsonSerializerSettings, 
            ITelegramService telegramService,
            IMapper mapper)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _telegramService = telegramService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Update(object updateObject)
        {
            var tmp = JsonConvert.SerializeObject(updateObject, _jsonSerializerSettings);

            var update = JsonConvert.DeserializeObject<Update>(tmp, _jsonSerializerSettings);

            var updateDto = _mapper.Map<UpdateDto>(update);

            await _telegramService.SavePhotoWithTags(updateDto);

            return Ok();
        }
    }
}