using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImageAPI.DatabaseContext
{
    public class ImageDbContext : DbContext
    {
        public DbSet<ImageDetails> Images { get; set; }
        public ImageDbContext(DbContextOptions<ImageDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageDetails>().ToTable("Images");
        }
    }
}
