using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.ViewModels;

public class ProductDetailViewModel {
    public List<ProductImageModel> Images { get; set; } = new List<ProductImageModel>();
    public ProductModel Product { get; set; }
    public GenderModel Gender { get; set; }
    public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    public List<ColorModel> AvailableColors { get; set; } = new List<ColorModel>();
    public List<SizeModel> AvailableSizes { get; set; } = new List<SizeModel>();
    
    public Dictionary<string, int> StockPerCombination { get; set; } = new Dictionary<string, int>();
    
    
}