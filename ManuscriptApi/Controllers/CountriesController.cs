using ManuscriptApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private static List<Country> Countries = new List<Country>
        {
            new Country { Id = 1, Name = "Poland", Description = "", IconUrl="https://flagcdn.com/16x12/ua.png" },
            new Country { Id = 2, Name = "England", Description = "", IconUrl="https://flagcdn.com/16x12/en.png" }
        };


        /// <summary>
        /// Endpoint for getting all the countries
        /// </summary>
        /// <returns>A list of Country objects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            return Ok(Countries);
        }

        /// <summary>
        /// Endpoint for adding a Country
        /// </summary>
        [HttpPost]
        public ActionResult<Country> AddCountry(Country country)
        {
            country.Id = Countries.Max(c => c.Id) + 1;

            Countries.Add(country);

            return CreatedAtAction(nameof(AddCountry), new {id = country.Id}, country);
        }

        /// <summary>
        /// Endpoint for getting a Country by its id
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Country> GetCountry(int id)
        {
            Country? country = Countries.FirstOrDefault(c => c.Id == id);

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
        public ActionResult UpdateCountry(int id, Country updatedCountry)
        {
            Country? country = Countries.FirstOrDefault(c => c.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            country.Name = updatedCountry.Name;
            country.Description = updatedCountry.Description;
            country.IconUrl = updatedCountry.IconUrl;
            
            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Country specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteCountry(int id)
        {
            Country? country = Countries.FirstOrDefault(c => c.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            Countries.Remove(country);
            return NoContent();
        }
    }
}
