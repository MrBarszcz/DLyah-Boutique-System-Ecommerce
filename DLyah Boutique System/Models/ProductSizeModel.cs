using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ProductSizeModel {
    public int ProductId { get; set; }
    public int SizeId { get; set; }
    public virtual ProductModel Product { get; set; } = null!;
    public virtual SizeModel Size { get; set; } = null!;
}