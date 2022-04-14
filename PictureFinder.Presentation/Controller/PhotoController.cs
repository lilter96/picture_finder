using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PictureFinder.Application.WebServices;
using PictureFinder.Presentation.Models;
using PictureFinder.Presentation.Models.Photo;

namespace PictureFinder.Presentation.Controller
{
    public class PhotoController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new PhotoResponseModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string tagName)
        {
            var photos = await _photoService.GetPhotosWithTagsByTagNameAsync(tagName);
            var model = new PhotoResponseModel
            {
                Photos = photos,
                SearchTagName = tagName
            };

            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> Delete([FromBody] DeleteByTagPhoto model)
        {
            var result = await _photoService.DeletePhotosByTagNameAsync(model.Tag);

            if (result) return Ok();

            return BadRequest();
        }
    }
}