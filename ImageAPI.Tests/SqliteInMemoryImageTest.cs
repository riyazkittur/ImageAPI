using ImageAPI.Services;
using System;
using Xunit;
using ImageAPI.DatabaseContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.Common;
using System.Collections.Generic;
using System.IO;

namespace ImageAPI.Tests
{
    public class SqliteInMemoryImageTest: ImagesDbContextTest,IDisposable
    {
        private readonly DbConnection _connection;

        public SqliteInMemoryImageTest()
            : base(
                new DbContextOptionsBuilder<ImageDbContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => _connection.Dispose();

        [Fact]
        public void Can_ShowAllImages_Available()
        {
            ImageService service = new ImageService(new ImageDbContext(ContextOptions));
            var imageList =  service.GetAllImageNames().Result;
            const int expected_ImagesCount = 3;
            Assert.Equal(expected_ImagesCount, imageList.Count);

        }
        [Fact]
        public void Can_SearchImages_By_Name()
        {
            ImageService service = new ImageService(new ImageDbContext(ContextOptions));
            var imageList = service.SearchImagesByName("Image").Result;
            const int expected_imagesCount = 2;
            Assert.Equal(expected_imagesCount, imageList.Count);
        }
        [Fact]
        public void Can_SortImages_AscendingOrder_PostedTime()
        {
            ImageService service = new ImageService(new ImageDbContext(ContextOptions));
            var imageList = service.GetImagesSortedByDate("asc").Result;
            List<string> expected_Images = new List<string>() { "Image2", "Image1", "Profile Picture" };
            Assert.Equal<List<string>>(
                expected_Images,
                imageList);
        }
        [Fact]
        public void Can_SortImages_DescendingOrder_PostedTime()
        {
            ImageService service = new ImageService(new ImageDbContext(ContextOptions));
            var imageList = service.GetImagesSortedByDate("des").Result;
            List<string> expected_Images = new List<string>() { "Profile Picture", "Image1", "Image2" };
            Assert.Equal<List<string>>(
                expected_Images,
                imageList);
        }
        [Fact]
        public void Can_Upload_Image()
        {
            ImageService service = new ImageService(new ImageDbContext(ContextOptions));
            byte[] fileContent = new byte[20];
            string fileName = "test_image.jpg";
            service.UploadImage(fileName, fileContent).GetAwaiter();
            var imageList = service.GetAllImageNames().Result;
            int IMAGES_COUNT_AFTER_UPLOAD = 4;
            Assert.Equal(IMAGES_COUNT_AFTER_UPLOAD, imageList.Count);
            var searchImageResult = service.SearchImagesByName(fileName).Result;
            Assert.Single(searchImageResult);


        }
        
        
    }
}
