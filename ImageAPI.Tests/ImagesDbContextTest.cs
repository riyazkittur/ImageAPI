using ImageAPI.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageAPI.Tests
{
    public class ImagesDbContextTest
    {
        protected DbContextOptions<ImageDbContext> ContextOptions { get; }
        public DbSet<ImageDetails> Images { get; set; }
        protected ImagesDbContextTest(DbContextOptions<ImageDbContext> options)
        {
            ContextOptions = options;
            Seed();
        }
        private void Seed()
        {
            using( var context = new ImageDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                ImageDetails imageDetails1 = new ImageDetails()
                {
                    Id = 1,
                    FileName = "Image1",
                    PostedDateTime = DateTime.UtcNow.AddDays(-1)
                };
                ImageDetails imageDetails2 = new ImageDetails()
                {
                    Id = 2,
                    FileName = "Image2",
                    PostedDateTime = DateTime.UtcNow.AddDays(-2)
                };
                ImageDetails imageDetails3 = new ImageDetails()
                {
                    Id = 3,
                    FileName = "Profile Picture",
                    PostedDateTime = DateTime.UtcNow
                };
                context.Images.AddRange(imageDetails1, imageDetails2, imageDetails3);
                context.SaveChanges();
            }

        }
    }
}
