using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PictureFinder.Application.Exceptions;

namespace PictureFinder.Presentation.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string errorMessage;

            switch (exception)
            {
                case UpdateObjectDoesNotContainPhotoException ex:
                    statusCode = 200;
                    errorMessage =
                        $"The update object does not contain photos.\nMessage - {ex.Message}\nException {JsonConvert.SerializeObject(ex)}";
                    break;
                default:
                    statusCode = 500;
                    errorMessage = "Internal server error.";
                    break;
            }

            await context.Response.WriteErrorAsync(errorMessage, statusCode);
        }
    }
}