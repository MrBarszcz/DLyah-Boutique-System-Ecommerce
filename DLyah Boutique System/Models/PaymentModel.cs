using System;

namespace DLyah_Boutique_System.Models;

public class PaymentModel {
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public decimal PaymentValuePaid { get; set; }
    public DateTime? PaymentDate { get; set; }
    public virtual OrderModel Order { get; set; } = null!;
    
    public ICollection<UserPaymentModel> UserPayments { get; set; }
}