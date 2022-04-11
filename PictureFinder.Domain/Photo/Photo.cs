using System;
using System.Collections.Generic;
using PictureFinder.Domain.Base;

namespace PictureFinder.Domain.Photo
{
    public class Photo : IEntity
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public List<Tag.Tag> Tags { get; set; }
    }
}