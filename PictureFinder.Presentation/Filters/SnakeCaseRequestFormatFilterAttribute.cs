using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PictureFinder.Presentation.Filters
{
    public class SnakeCaseRequestFormatFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var mvcOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value;
            var inputFormatters = mvcOptions.InputFormatters;
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<SnakeCaseRequestFormatFilterAttribute>>();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            inputFormatters.Clear();

            inputFormatters.Add(
                new NewtonsoftJsonInputFormatter(
                    logger,
                    jsonSerializerSettings,
                    ArrayPool<char>.Shared,
                    new DefaultObjectPoolProvider(),
                    mvcOptions,
                    new MvcNewtonsoftJsonOptions()
                ));

            var executedContext = await next();
        }
    }
}