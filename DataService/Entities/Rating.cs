using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Rating
{
    public string Id { get; set; } = null!;

    public double? RatingPoint { get; set; }

    public string? Comment { get; set; }

    public string? ImgUrl { get; set; }

    public string? Reciver { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<WorkMapping> WorkMappings { get; } = new List<WorkMapping>();
}
