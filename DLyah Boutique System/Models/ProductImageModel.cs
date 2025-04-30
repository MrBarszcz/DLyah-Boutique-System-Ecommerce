using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ProductImageModel {
    public int ProductImageId { get; set; }
    public int ProductId { get; set; }
    public string ProductImagePath { get; set; } = null!;
    public int? ImageOrder { get; set; }
    public virtual ProductModel Product { get; set; } = null!;
}