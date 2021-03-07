using ImageAPI.DatabaseContext;
using ImageAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageDbContext _dbContext;
        public ImageService(ImageDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public Task<List<string>> GetAllImageNames()
        {
            return _dbContext.Images
                 .Select(s => s.FileName).ToListAsync();
        }

        public Task<List<string>> SearchImagesByName(string fileName)
        {
            return _dbContext.Images
                .Where(i => i.FileName.ToUpper().Contains(fileName.ToUpper()))
                .Select(s => s.FileName).ToListAsync();
        }

        public async Task UploadImage(string fileName, byte[] fileContent)
        {
            _dbContext.Images.Add(new ImageDetails()
            {
                FileName = fileName,
                FileContent=fileContent,
                PostedDateTime = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
            
        }
        public Task<List<string>> GetImagesSortedByDate(string sortOrder="asc")
        {
            if (sortOrder == "des")
            {
                return _dbContext.Images
                               .OrderByDescending(i => i.PostedDateTime)
                               .Select(s => s.FileName)
                               .ToListAsync<string>();
            }
            else
            {
                
                return _dbContext.Images
                                  .OrderBy(i => i.PostedDateTime)
                                  .Select(s => s.FileName)
                                  .ToListAsync<string>();

            }

        }
    }
}
