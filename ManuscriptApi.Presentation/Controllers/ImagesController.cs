using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService  _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Get all images (excluding soft-deleted).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            var images = await _imageService.GetAllAsync();
            return Ok(images);
        }

        /// <summary>
        /// Get an image by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null)
                return NotFound();

            return Ok(image);
        }

        /// <summary>
        /// Add a new image.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Image>> AddImage(ImageDto imageDto)
        {
            var image = await _imageService.CreateAsync(imageDto);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
        }

        /// <summary>
        /// Update an image by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, ImageDto imageDto)
        {
            var updated = await _imageService.UpdateAsync(id, imageDto);
            if (!updated)
                return BadRequest("Could not update image");

            return NoContent();
        }

        /// <summary>
        /// Soft-delete an image by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var deleted = await _imageService.DeleteAsync(id);
            if (!deleted)
                return BadRequest("Could not delete image");

            return NoContent();
        }
    }
}
