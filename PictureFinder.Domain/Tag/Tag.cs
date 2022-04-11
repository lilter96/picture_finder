using System;
using System.Collections.Generic;
using PictureFinder.Domain.Base;

namespace PictureFinder.Domain.Tag
{
    public class Tag : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Photo.Photo> Photos { get; set; }
    }
}