using AutoMapper;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManuscriptsController : ControllerBase
    {
        private readonly ICrudService<Manuscript> _manuscriptService;
        private readonly IMapper _mapper;

        public ManuscriptsController(ICrudService<Manuscript> manuscriptService, IMapper mapper)
        {
            _manuscriptService = manuscriptService;
            _mapper = mapper;
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
            if (manuscript == null)
                return NotFound();

            return Ok(manuscript);
        }

        /// <summary>
        /// Creates a new manuscript
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Manuscript>> Create(ManuscriptDto dto)
        {
            var manuscript = _mapper.Map<Manuscript>(dto);
            var created = await _manuscriptService.CreateAsync(manuscript);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing manuscript
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ManuscriptDto dto)
        {
            var entity = _mapper.Map<Manuscript>(dto);
            var updated = await _manuscriptService.UpdateAsync(entity, id);

            if (updated == null)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes (soft deletes) a manuscript
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _manuscriptService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
