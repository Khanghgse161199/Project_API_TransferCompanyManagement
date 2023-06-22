using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class CategoryBlock
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public virtual ICollection<Block> Blocks { get; } = new List<Block>();
}
