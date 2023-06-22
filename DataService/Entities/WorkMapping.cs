using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class WorkMapping
{
    public string Id { get; set; } = null!;

    public string? EmployeeId { get; set; }

    public string TransitCarId { get; set; } = null!;

    public string ContainerId { get; set; } = null!;

    public string RatingId { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int Status { get; set; }

    public virtual Container Container { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderShipping> OrderShippings { get; } = new List<OrderShipping>();

    public virtual Rating Rating { get; set; } = null!;

    public virtual TransitCar TransitCar { get; set; } = null!;
}
