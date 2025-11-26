using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.ViewModels;

public class HomeViewModel {
    public List<BannerModel> TopBanners { get; set; } = new List<BannerModel>();
    
    public List<ProductModel> ShowcaseProducts { get; set; } = new List<ProductModel>();
}