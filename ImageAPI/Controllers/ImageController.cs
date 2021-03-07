using ImageAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private const string _imageFormat= "[^\\s]+(.*?)\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$";
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [Route("~/")]
        [HttpPost]
        ////Upload image to database
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
                string fileName = file.FileName;

               if( Regex.Match(fileName, _imageFormat).Success)
                {
                    byte[] fileContent;
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        fileContent = ms.ToArray();
                    }
                    await _imageService.UploadImage(fileName, fileContent);
                    return StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest
                        , "Invalid Image format.Supporting image formats  are .jpg,.jpeg,.png,.gif");
                }
               
            }

            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
        [Route("~/")]
        [HttpGet]
        ////Get All Image names list available from database
        public async Task<IActionResult> GetAllImages()
        {
            List<string> listImages;
            try
            {
                listImages = await _imageService.GetAllImageNames();
                if(listImages==null || listImages.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listImages);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        [Route("~/asc")]
        [HttpGet]
        ////Get all image names list sorted in ascending order of posted date time
        public async Task<IActionResult> GetAllImagesInAscendingOrderofUploadTime()
        {
            List<string> listImages;
            try
            {
                listImages = await _imageService.GetImagesSortedByDate("asc");
                if (listImages == null || listImages.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listImages);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("~/des")]
        [HttpGet]
        ////Get all image names list sorted in descending order of posted date time
        public async Task<IActionResult> GetAllImagesInDescendingOrderofUploadTime()
        {
            List<string> listImages;
            try
            {
                listImages = await _imageService.GetImagesSortedByDate("des");
                if (listImages == null || listImages.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listImages);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        [Route("~/search")]
        [HttpPost]
        ////Get all image names list containing searchKeyword in name(case insensitive search)
        public async Task<IActionResult> GetAllImagesByKeyWord([FromBody]string searchKeyword)
        {
            List<string> listImages;
            try
            {
                if (String.IsNullOrEmpty(searchKeyword))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
                listImages = await _imageService.SearchImagesByName(searchKeyword);
                if (listImages == null || listImages.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listImages);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }

    }
}
