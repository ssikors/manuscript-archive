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

        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            return Ok(Countries);
        }

        [HttpPost]
        public ActionResult<Country> AddCountry(Country country)
        {
            country.Id = Countries.Max(c => c.Id) + 1;

            Countries.Add(country);

            return CreatedAtAction(nameof(AddCountry), new {id = country.Id}, country);
        }

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
