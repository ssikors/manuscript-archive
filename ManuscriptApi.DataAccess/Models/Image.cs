using System;
using System.Collections.Generic;

public class Image
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string Url { get; set; } = null!;

    public int ManuscriptId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Manuscript Manuscript { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
