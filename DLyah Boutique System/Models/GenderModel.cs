using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class GenderModel {
    public int GenderId { get; set; }
    public string Gender { get; set; } = null!;
    public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
}