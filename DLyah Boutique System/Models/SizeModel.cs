using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class SizeModel {
    public int SizeId { get; set; }
    public string Size { get; set; } = null!;
    public virtual ICollection<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
    public virtual ICollection<StockProductModel> StockProducts { get; set; } = new List<StockProductModel>();
}