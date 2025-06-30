using DLyah_Boutique_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DLyah_Boutique_System.ViewModels;

public class BannerPlacementViewModel {
    public string PageName { get; set; }
        
    // Lista dos banners que já estão na página
    public List<BannerPlacementModel> CurrentPlacements { get; set; } = new List<BannerPlacementModel>();
        
    // Lista de banners disponíveis para adicionar
    public List<BannerModel> AvailableBanners { get; set; } = new List<BannerModel>();
        
    // Propriedades para o formulário de adição
    public int BannerIdToAdd { get; set; }
    
    public string PositionToAdd { get; set; }
    
    public List<SelectListItem> AvailablePositions { get; set; } = new List<SelectListItem>();
}