
using System.Xml.Linq;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Endpoint for getting all the Tags
        /// </summary>
        /// <returns>A list of Tag objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        /// <summary>
        /// Endpoint for adding a Tag
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Tag>> AddTag(TagDto tagDto)
        {
            var tag = await _tagService.CreateAsync(tagDto);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        /// <summary>
        /// Endpoint for getting a Tag by its id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        /// <summary>
        /// Endpoint for updating a Tag specified by id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, TagDto updatedTag)
        {
            var success = await _tagService.UpdateAsync(id, updatedTag);

            if (!success)
                return BadRequest("Could not update the tag");

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Tag specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var success = await _tagService.DeleteAsync(id);

            if (!success)
                return BadRequest("Could not delete the tag");

            return NoContent();
        }
    }
}
