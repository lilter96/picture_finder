using System;
using System.Linq;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace PictureFinder.Presentation.ExceptionHandling
{
    public static class ErrorHttpResponseExtensions
    {
        public static async Task WriteErrorAsync(this HttpResponse httpResponse, string error, int statusCode)
        {
            httpResponse.Clear();
            httpResponse.ContentType = "application/json";
            httpResponse.StatusCode = statusCode;
            await httpResponse.WriteAsync(error);
        }
    }
}