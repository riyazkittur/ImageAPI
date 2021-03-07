using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Interfaces
{
    public interface IImageService
    {
        Task UploadImage(string fileName,byte[] fileContent);
        Task<List<string>> SearchImagesByName(string fileName);
        Task<List<string>> GetAllImageNames();
        Task<List<string>> GetImagesSortedByDate(string sortOrder);
    }
}
