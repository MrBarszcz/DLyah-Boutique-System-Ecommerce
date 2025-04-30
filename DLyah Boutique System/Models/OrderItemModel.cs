using System;

namespace DLyah_Boutique_System.Models;

public class OrderItemModel {
    public int ItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int OrderQuantity { get; set; }
    public decimal OrderPriceUnitary { get; set; }
    public decimal Subtotal { get; } // Computed property, EF Core can handle this
    public virtual OrderModel Order { get; set; } = null!;
    public virtual ProductModel Product { get; set; } = null!;
}