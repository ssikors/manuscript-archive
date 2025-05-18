using ManuscriptApi.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManuscriptsController : ControllerBase
    {
        private static List<Manuscript> Manuscripts = new List<Manuscript>
        {
            new Manuscript { Id = 1, Name = "Le Romant des Fables Ovide le Grant", Description = "", AuthorId = 1, CountryId = 2, Tags = [new Tag { Id = 1, Name = "Martial", Description = "", SubTags = [] }], Url = "https://gallica.bnf.fr/ark:/12148/btv1b525031179/f9.item", YearWritten = 1330},
            new Manuscript { Id = 2, Name = "Hedwig Manuscript", Description = "", AuthorId = 2, CountryId = 1, Tags = [], Url = "http://commons.wikimedia.org/wiki/File:HedwigManuscriptLiegnitz_b.jpg", YearWritten = 1451},
        };


        /// <summary>
        /// Endpoint for getting all the manuscripts
        /// </summary>
        /// <returns>A list of Manuscript objects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Manuscript>> GetManuscripts()
        {
            return Ok(Manuscripts);
        }

        /// <summary>
        /// Endpoint for adding a Manuscript
        /// </summary>
        [HttpPost]
        public ActionResult<Manuscript> AddManuscript(Manuscript Manuscript)
        {
            Manuscript.Id = Manuscripts.Max(c => c.Id) + 1;

            Manuscripts.Add(Manuscript);

            return CreatedAtAction(nameof(AddManuscript), new { id = Manuscript.Id }, Manuscript);
        }

        /// <summary>
        /// Endpoint for getting a Manuscript by its id
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Manuscript> GetManuscript(int id)
        {
            Manuscript? Manuscript = Manuscripts.FirstOrDefault(c => c.Id == id);

            if (Manuscript == null)
            {
                return NotFound();
            }

            return Ok(Manuscript);
        }

        /// <summary>
        /// Endpoint for updating a Manuscript specified by id
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateManuscript(int id, Manuscript updatedManuscript)
        {
            Manuscript? Manuscript = Manuscripts.FirstOrDefault(c => c.Id == id);

            if (Manuscript == null)
            {
                return NotFound();
            }

            Manuscript.Name = updatedManuscript.Name;
            Manuscript.Description = updatedManuscript.Description;
            Manuscript.Url = updatedManuscript.Url;
            Manuscript.AuthorId = updatedManuscript.AuthorId;
            Manuscript.CountryId = updatedManuscript.CountryId;
            Manuscript.YearWritten = updatedManuscript.YearWritten;
            Manuscript.Tags = updatedManuscript.Tags;

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Manuscript specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteManuscript(int id)
        {
            Manuscript? Manuscript = Manuscripts.FirstOrDefault(c => c.Id == id);

            if (Manuscript == null)
            {
                return NotFound();
            }

            Manuscripts.Remove(Manuscript);
            return NoContent();
        }
    }
}
