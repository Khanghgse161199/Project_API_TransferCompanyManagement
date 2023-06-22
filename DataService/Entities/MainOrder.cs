using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class MainOrder
{
    public string Id { get; set; } = null!;

    public string Reciver { get; set; } = null!;

    public string ReciverPhone { get; set; } = null!;

    public string ReciverEmail { get; set; } = null!;

    public string ReciverAddress { get; set; } = null!;

    public bool IsDone { get; set; }

    public string Sender { get; set; } = null!;

    public string ReciverCitizenId { get; set; } = null!;

    public decimal Total { get; set; }

    public DateTime DateTimeCreate { get; set; }

    public DateTime? DateTimeDone { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<OrderShipping> OrderShippings { get; } = new List<OrderShipping>();
}
