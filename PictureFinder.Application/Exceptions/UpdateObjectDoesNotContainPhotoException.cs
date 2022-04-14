using System;

namespace PictureFinder.Application.Exceptions
{
    public class UpdateObjectDoesNotContainPhotoException : Exception
    {
        public UpdateObjectDoesNotContainPhotoException(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}