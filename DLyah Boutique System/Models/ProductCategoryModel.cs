using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ProductCategoryModel {
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public virtual ProductModel Product { get; set; } = null!;
    public virtual CategoryModel Category { get; set; } = null!;
}