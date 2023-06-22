using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Token
{
    public string Id { get; set; } = null!;

    public string AccId { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string EmailToken { get; set; } = null!;

    public DateTime EtcreatedDate { get; set; }

    public DateTime CreateDate { get; set; }

    public bool AccessTokenIsActive { get; set; }

    public bool EtisActive { get; set; }

    public virtual Account Acc { get; set; } = null!;
}
