using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PictureFinder.Application.Dto;
using PictureFinder.Application.WebServices;
using PictureFinder.Integration.Telegram.Models;
using PictureFinder.Presentation.Filters;

namespace PictureFinder.Presentation.Controller
{
    [ApiController]
    [Route("telegram")]
    public class TelegramController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITelegramService _telegramService;

        public TelegramController(
            ITelegramService telegramService,
            IMapper mapper)
        {
            _telegramService = telegramService;
            _mapper = mapper;
        }

        [HttpPost]
        [SnakeCaseRequestFormatFilter]
        public async Task<IActionResult> Update(Update updateRequest)
        {
            var updateDto = _mapper.Map<UpdateDto>(updateRequest);

            await _telegramService.SavePhotoWithTagsAsync(updateDto);

            return Ok();
        }
    }
}