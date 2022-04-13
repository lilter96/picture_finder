using System;

namespace PictureFinder.Application.Exceptions
{
    public class UpdateObjectDoesNotContainPhotoException : Exception
    {
        public override string Message { get; }

        public UpdateObjectDoesNotContainPhotoException(string message)
        {
            Message = message;
        }
    }
}