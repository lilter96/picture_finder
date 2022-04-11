using System;

namespace PictureFinder.Application.Exceptions
{
    public class UpdateObjectDoesNotContainPhotoException : Exception
    {
        public string Message { get; }

        public UpdateObjectDoesNotContainPhotoException(string message)
        {
            Message = message;
        }
    }
}