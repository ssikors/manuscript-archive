using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
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
            var location = await _locationService.CreateAsync(locationDto);
            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
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
            var updated = await _locationService.UpdateAsync(id, updatedLocation);

            if (!updated)
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
