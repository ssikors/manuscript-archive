using System.Security.Claims;
using AutoMapper;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using ManuscriptApi.Domain.Services;
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
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var locations = await _locationService.GetAllAsync(email);
            return Ok(locations);
        }

        /// <summary>
        /// Endpoint for adding a Location.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(LocationDto locationDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var location = _mapper.Map<Location>(locationDto);
            var created = await _locationService.CreateAsync(location, email);

            return CreatedAtAction(nameof(GetLocation), new { id = created.Id }, created);
        }

        /// <summary>
        /// Endpoint for getting a Location by its id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var location = await _locationService.GetByIdAsync(id, email);

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
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var entity = _mapper.Map<Location>(updatedLocation);
            var updated = await _locationService.UpdateAsync(entity, id, email);

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
            var email = User.FindFirst(ClaimTypes.Email)!.Value;

            var deleted = await _locationService.DeleteAsync(id, email);

            if (!deleted)
            {
                return BadRequest("Could not delete location");
            }

            return NoContent();
        }
    }
}
