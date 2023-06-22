using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class CategoryContainer
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public virtual ICollection<Container> Containers { get; } = new List<Container>();
}
