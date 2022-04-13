using Microsoft.EntityFrameworkCore;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Tag;

namespace PictureFinder.Data.Sql
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Photo>().HasKey(photo => photo.Id);
            builder.Entity<Photo>().HasIndex(photo => photo.MediaGroupId);
            builder.Entity<Photo>().HasMany(x => x.Tags).WithMany(y => y.Photos);

            builder.Entity<Tag>().HasKey(tag => tag.Id);
        }
    }
}