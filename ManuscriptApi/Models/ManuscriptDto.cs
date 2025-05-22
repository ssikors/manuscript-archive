namespace ManuscriptApi.Models
{
    public class ManuscriptDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int YearWritten { get; set; }
        public string Url { get; set; }

        public int CountryId { get; set; }
        public int AuthorId { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
