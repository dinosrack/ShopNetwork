using System;
using System.Collections.Generic;

namespace ShopNetwork.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
