using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ColorModel {
    public int ColorId { get; set; }
    public string Color { get; set; } = null!;
    public string HexColor { get; set; } = null!;
    public virtual ICollection<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
    public virtual ICollection<StockProductModel> StockProducts { get; set; } = new List<StockProductModel>();
}