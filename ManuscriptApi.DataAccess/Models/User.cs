using System;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsModerator { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Manuscript> Manuscripts { get; set; } = new List<Manuscript>();
}
