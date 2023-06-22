using System;
using System.Collections.Generic;

namespace DataService.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; } = new List<Token>();
}
