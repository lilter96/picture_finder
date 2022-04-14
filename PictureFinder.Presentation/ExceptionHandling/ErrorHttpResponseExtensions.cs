using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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