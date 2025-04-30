using System;
using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ClientModel {
    public int ClientId { get; set; }
    public string ClientCpf { get; set; } = null!;
    public string ClientPhonenumber { get; set; } = null!;
    public DateTime? ClientDateBirth { get; set; }
    public string ClientAddress { get; set; } = null!;
    public string ClientCity { get; set; } = null!;
    public string ClientState { get; set; } = null!;
    public string ClientCep { get; set; } = null!;
    public DateTime? ClientDateRegister { get; set; }
    public string ClientStatus { get; set; } = null!;
    public int UserId { get; set; }
    public virtual UserModel User { get; set; } = null!;
    public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
}