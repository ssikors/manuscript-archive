using ManuscriptApi.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private static List<Tag> Tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "Martial", Description = "", SubTags = [] },
            new Tag { Id = 2, Name = "Civillian", Description = "", SubTags = []}
        };


        /// <summary>
        /// Endpoint for getting all the Tags
        /// </summary>
        /// <returns>A list of Tag objects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Tag>> GetTags()
        {
            return Ok(Tags);
        }

        /// <summary>
        /// Endpoint for adding a Tag
        /// </summary>
        [HttpPost]
        public ActionResult<Tag> AddTag(Tag Tag)
        {
            Tag.Id = Tags.Max(c => c.Id) + 1;

            Tags.Add(Tag);

            return CreatedAtAction(nameof(AddTag), new { id = Tag.Id }, Tag);
        }

        /// <summary>
        /// Endpoint for getting a Tag by its id
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Tag> GetTag(int id)
        {
            Tag? Tag = Tags.FirstOrDefault(t => t.Id == id);

            if (Tag == null)
            {
                return NotFound();
            }

            return Ok(Tag);
        }

        /// <summary>
        /// Endpoint for updating a Tag specified by id
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateTag(int id, Tag updatedTag)
        {
            Tag? Tag = Tags.FirstOrDefault(t => t.Id == id);

            if (Tag == null)
            {
                return NotFound();
            }

            Tag.Name = updatedTag.Name;
            Tag.Description = updatedTag.Description;
            Tag.SubTags = updatedTag.SubTags;

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a Tag specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteTag(int id)
        {
            Tag? Tag = Tags.FirstOrDefault(t => t.Id == id);

            if (Tag == null)
            {
                return NotFound();
            }

            Tags.Remove(Tag);
            return NoContent();
        }
    }
}
