

public class Country : IModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string IconUrl { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
