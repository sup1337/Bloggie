using System.Net;
using Bloggie.web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //call repo
            var immageURL = await imageRepository.UploadAsync(file);
            if (immageURL == null)
            {
                return Problem("Something went wrong while uploading the image",null ,statusCode: (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new {link = immageURL});
        }
    }
}
