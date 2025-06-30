using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.ViewModels;

public class HomeViewModel {
    public List<BannerModel> TopBanners { get; set; } = new List<BannerModel>();
        
    // Usaremos esta para a vitrine de produtos
    public List<ProductModel> ShowcaseProducts { get; set; } = new List<ProductModel>();
}