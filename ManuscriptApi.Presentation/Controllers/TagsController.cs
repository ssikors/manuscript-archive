using System.Security.Claims;
using AutoMapper;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using ManuscriptApi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ICrudService<Tag> _tagService;
        private readonly IMapper _mapper;

        public TagsController(ICrudService<Tag> tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint for getting all the Tags
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var tags = await _tagService.GetAllAsync(email);
            return Ok(tags);
        }

        /// <summary>
        /// Endpoint for adding a Tag
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Tag>> AddTag(TagDto tagDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var tag = _mapper.Map<Tag>(tagDto);
            var created = await _tagService.CreateAsync(tag, email);
            return CreatedAtAction(nameof(GetTag), new { id = created.Id }, created);
        }

        /// <summary>
        /// Endpoint for getting a Tag by its id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var tag = await _tagService.GetByIdAsync(id, email);
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
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var entity = _mapper.Map<Tag>(updatedTag);
            var updated = await _tagService.UpdateAsync(entity, id, email);

            if (updated == null)
                return BadRequest("Could not update the tag");

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Tag specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var deleted = await _tagService.DeleteAsync(id, email);

            if (!deleted)
                return BadRequest("Could not delete the tag");

            return NoContent();
        }
    }
}
