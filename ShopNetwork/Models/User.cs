using System;
using System.Collections.Generic;

namespace ShopNetwork.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserRole { get; set; } = null!;
}
