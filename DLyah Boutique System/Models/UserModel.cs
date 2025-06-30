using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DLyah_Boutique_System.Models;

public class UserModel : IdentityUser<int> {
    public int UserId { get; set; }
    public string UserNameComplete { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    // public string UserPassword { get; set; } = null!;
    public string UserType { get; set; } = null!;
    public DateTime? UserDateRegister { get; set; }
    public virtual ClientModel? Client { get; set; }
    public virtual ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
    public virtual ICollection<UserPaymentModel> UserPayments { get; set; } = new List<UserPaymentModel>();
    public virtual UserProfileImageModel? UserProfileImage { get; set; }
}