using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ProductColorModel {
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public virtual ProductModel Product { get; set; } = null!;
    public virtual ColorModel Color { get; set; } = null!;
}