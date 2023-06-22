using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Container
{
    public string Id { get; set; } = null!;

    public string CategoryTransId { get; set; } = null!;

    public double Weight { get; set; }

    public bool IsActve { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? LastUpdate { get; set; }

    public bool IsWorking { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public virtual CategoryContainer CategoryTrans { get; set; } = null!;

    public virtual ICollection<WorkMapping> WorkMappings { get; } = new List<WorkMapping>();
}
