using System;

namespace DLyah_Boutique_System.Models;

public class OrderModel {
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public DateTime? DateOrder { get; set; }
    public string OrderStatus { get; set; } = null!;
    public decimal OrderValueTotal { get; set; }
    public int? PaymentId { get; set; }
    public virtual ClientModel Client { get; set; } = null!;
    public virtual ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    public virtual PaymentModel? Payment { get; set; }
}