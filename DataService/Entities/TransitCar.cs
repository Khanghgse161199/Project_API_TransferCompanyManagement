using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class TransitCar
{
    public string Id { get; set; } = null!;

    public DateTime DateRegister { get; set; }

    public string Name { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public bool IsActive { get; set; }

    public string OriginCompany { get; set; } = null!;

    public DateTime OutOfDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public bool IsWorking { get; set; }

    public virtual ICollection<WorkMapping> WorkMappings { get; } = new List<WorkMapping>();
}
