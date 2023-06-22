using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Employee
{
    public string Id { get; set; } = null!;

    public string AccId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public double SummayRating { get; set; }

    public bool IsWorking { get; set; }

    public virtual Account Acc { get; set; } = null!;

    public virtual ICollection<WorkMapping> WorkMappings { get; } = new List<WorkMapping>();
}
