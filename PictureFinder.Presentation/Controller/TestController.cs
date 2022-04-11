using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TL;
using Message = PictureFinder.Integration.Telegram.Models.Message;
using PhotoSize = PictureFinder.Integration.Telegram.Models.PhotoSize;
using Update = PictureFinder.Integration.Telegram.Models.Update;

namespace PictureFinder.Presentation.Controller
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            var a = new Update
            {
                Message = new Message
                {
                    Photo = new List<PhotoSize>
                        {
                            new PhotoSize
                            {
                                FileId = "123123",
                                FileSize = 2000,
                                FileUniqueId = "123123123123",
                                Height = 20,
                                Width = 30
                            }
                        }
                    }
            };
            return Ok(JsonConvert.SerializeObject(a, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            }));
        }
    }
}