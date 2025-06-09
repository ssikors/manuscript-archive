
public class Tag : IModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Manuscript> Manuscripts { get; set; } = new List<Manuscript>();
}
