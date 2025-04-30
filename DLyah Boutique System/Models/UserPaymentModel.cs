namespace DLyah_Boutique_System.Models;

public class UserPaymentModel {
    public int UserId { get; set; }
    public int PaymentId { get; set; }
    public virtual UserModel User { get; set; } = null!;
    public virtual PaymentModel Payment { get; set; } = null!;
}