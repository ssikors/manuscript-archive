
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private ICountryService _countryService;


        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// Endpoint for getting all the countries
        /// </summary>
        /// <returns>A list of Country objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            var countries = await _countryService.GetAllAsync();

            return Ok(countries);
        }

        /// <summary>
        /// Endpoint for adding a Country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Country>> AddCountry(CountryDto countryDto)
        {
            Country? country = await _countryService.CreateAsync(countryDto);

            return CreatedAtAction(nameof(AddCountry), new {id = country.Id}, country);
        }

        /// <summary>
        /// Endpoint for getting a Country by its id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            Country? country = await _countryService.GetByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        /// <summary>
        /// Endpoint for updating a Country specified by id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCountry(int id, CountryDto updatedCountry)
        {
            bool updated = await _countryService.UpdateAsync(id, updatedCountry);

            if (!updated)
            {
                return BadRequest("Could not update the country");
            }
            
            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Country specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCountry(int id)
        {
            bool deleted = await _countryService.DeleteAsync(id);

            if (!deleted)
            {
                return BadRequest("Could not delete country");
            }

            return NoContent();
        }
    }
}
