
public class Location : IModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Manuscript> Manuscripts { get; set; } = new List<Manuscript>();
}
