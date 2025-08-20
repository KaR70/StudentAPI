using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadControllerController : ControllerBase
    {
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file Uploaded");
            }
            var uploadDirectory = @"D:\C#\Restful API Mohammed Abu-Hadhoud\New folder\Photos";

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new {filePath});
        }

        [HttpGet("GetImage/{fileName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string fileName)
        {
            string uploadDirectory = @"D:\C#\Restful API Mohammed Abu-Hadhoud\New folder\Photos";
            string filePath = Path.Combine(uploadDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image Not Found");
            }

            var image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);

            return File(image, mimeType);
        }

        private string GetMimeType(string filepath)
        {
            string extention = Path.GetExtension(filepath).ToLowerInvariant();
            return extention switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
