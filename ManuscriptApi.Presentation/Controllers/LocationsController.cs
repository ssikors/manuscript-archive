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
    public class LocationsController : ControllerBase
    {
        private readonly ICrudService<Location> _locationService;
        private readonly IMapper _mapper;

        public LocationsController(ICrudService<Location> locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint for getting all the locations.
        /// </summary>
        /// <returns>A list of Location objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _locationService.GetAllAsync();
            return Ok(locations);
        }

        /// <summary>
        /// Endpoint for adding a Location.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(LocationDto locationDto)
        {
            var location = _mapper.Map<Location>(locationDto);
            var created = await _locationService.CreateAsync(location);

            return CreatedAtAction(nameof(GetLocation), new { id = created.Id }, created);
        }

        /// <summary>
        /// Endpoint for getting a Location by its id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _locationService.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        /// <summary>
        /// Endpoint for updating a Location specified by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLocation(int id, LocationDto updatedLocation)
        {
            var entity = _mapper.Map<Location>(updatedLocation);
            var updated = await _locationService.UpdateAsync(entity, id);

            if (updated == null)
            {
                return BadRequest("Could not update the location");
            }

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Location specified by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var deleted = await _locationService.DeleteAsync(id);

            if (!deleted)
            {
                return BadRequest("Could not delete location");
            }

            return NoContent();
        }
    }
}
