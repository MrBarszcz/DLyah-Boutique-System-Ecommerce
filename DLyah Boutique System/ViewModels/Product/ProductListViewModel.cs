using DLyah_Boutique_System.Models;
using System.Collections.Generic;

namespace DLyah_Boutique_System.ViewModels
{
    public class ProductListViewModel
    {
        public ProductModel Product { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<ColorModel> Colors { get; set; }
        public List<SizeModel> Sizes { get; set; }
        public GenderModel Gender { get; set; }
        public List<ProductImageModel> Images { get; set; }
        
        public List<StockProductModel> StockProducts { get; set; }
    }
}