using AutoMapper;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly ICrudService<Image> _imageService;
        private readonly IMapper _mapper;

        public ImagesController(ICrudService<Image> imageService, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
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
            var image = _mapper.Map<Image>(imageDto);
            var created = await _imageService.CreateAsync(image);

            return CreatedAtAction(nameof(GetImage), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an image by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, ImageDto imageDto)
        {
            var entity = _mapper.Map<Image>(imageDto);
            var updated = await _imageService.UpdateAsync(entity, id);

            if (updated == null)
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

