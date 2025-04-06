using System;
using System.Collections.Generic;

namespace ShopNetwork.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public decimal Weight { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
