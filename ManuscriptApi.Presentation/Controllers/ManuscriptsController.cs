
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptsController : ControllerBase
    {
        private readonly IManuscriptService _manuscriptService;

        public ManuscriptsController(IManuscriptService manuscriptService)
        {
            _manuscriptService = manuscriptService;
        }

        /// <summary>
        /// Gets all manuscripts
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manuscript>>> GetAll()
        {
            var manuscripts = await _manuscriptService.GetAllAsync();
            return Ok(manuscripts);
        }

        /// <summary>
        /// Gets a manuscript by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Manuscript>> GetById(int id)
        {
            var manuscript = await _manuscriptService.GetByIdAsync(id);
            if (manuscript == null) return NotFound();

            return Ok(manuscript);
        }

        /// <summary>
        /// Creates a new manuscript
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Manuscript>> Create(ManuscriptDto dto)
        {
            var manuscript = await _manuscriptService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = manuscript.Id }, manuscript);
        }

        /// <summary>
        /// Updates an existing manuscript
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ManuscriptDto dto)
        {
            var updated = await _manuscriptService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes (soft deletes) a manuscript
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _manuscriptService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
