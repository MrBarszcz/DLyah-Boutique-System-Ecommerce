using System;

namespace DLyah_Boutique_System.Models;

public class AddressModel {
    public int AddressId { get; set; }
    public int UserId { get; set; }
    public string AddressNumber { get; set; } = null!;
    public string? AddressComplement { get; set; }
    public string AddressNeighborhood { get; set; } = null!;
    public string AddressCity { get; set; } = null!;
    public string AddressState { get; set; } = null!;
    public string AddressCep { get; set; } = null!;
    public string AddressType { get; set; } = null!;
    public virtual UserModel User { get; set; } = null!;
}