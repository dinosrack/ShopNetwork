using System;
using System.Collections.Generic;

namespace ShopNetwork.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string DirectorLastName { get; set; } = null!;

    public int EmployeeCount { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
