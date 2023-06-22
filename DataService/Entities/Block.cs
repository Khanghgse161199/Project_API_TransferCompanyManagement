using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Block
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public double Weight { get; set; }

    public string CategoryBlockId { get; set; } = null!;

    public DateTime DateTimeCreate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual CategoryBlock CategoryBlock { get; set; } = null!;

    public virtual ICollection<OrderShipping> OrderShippings { get; } = new List<OrderShipping>();
}
