namespace ManuscriptApi.Business.DTOs
{
    public class ImageDto
    {
        public string? Title { get; set; }

        public string Url { get; set; } = null!;

        public int ManuscriptId { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

}
