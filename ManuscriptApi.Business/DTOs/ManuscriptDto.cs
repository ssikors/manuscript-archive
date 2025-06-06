namespace ManuscriptApi.Business.DTOs
{
    public class ManuscriptDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int YearWrittenStart { get; set; }
        public int? YearWrittenEnd { get; set; }
        public string? SourceUrl { get; set; }
        public int LocationId { get; set; }
        public int AuthorId { get; set; }
        public List<int>? TagIds { get; set; } = new();
    }
}
