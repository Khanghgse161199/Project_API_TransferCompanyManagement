using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class OrderShipping
{
    public string Id { get; set; } = null!;

    public string MainOrderId { get; set; } = null!;

    public bool IsActive { get; set; }

    public string BlockId { get; set; } = null!;

    public DateTime DateTimeCreate { get; set; }

    public string WorkMappingId { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime? DateTimeRecive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public bool IsDone { get; set; }

    public virtual Block Block { get; set; } = null!;

    public virtual MainOrder MainOrder { get; set; } = null!;

    public virtual WorkMapping WorkMapping { get; set; } = null!;
}
