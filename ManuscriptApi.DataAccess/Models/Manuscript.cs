using System;
using System.Collections.Generic;
using ManuscriptApi.DataAccess.Models;

public class Manuscript : IModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int YearWrittenStart { get; set; }

    public int? YearWrittenEnd { get; set; }

    public string? SourceUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int LocationId { get; set; }

    public int AuthorId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
