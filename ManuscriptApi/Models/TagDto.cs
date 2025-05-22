namespace ManuscriptApi.Models
{
    public class TagDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Tag> SubTags { get; set; }
    }
}
