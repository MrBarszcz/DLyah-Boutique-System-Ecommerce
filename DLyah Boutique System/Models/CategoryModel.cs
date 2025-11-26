using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class CategoryModel {
    public int CategoryId { get; set; }
    
    public string Category { get; set; } = null!;
    
    public virtual ICollection<ProductCategoryModel> ProductCategories { get; set; } = new List<ProductCategoryModel>();
}