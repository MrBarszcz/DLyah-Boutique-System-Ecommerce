using DLyah_Boutique_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DLyah_Boutique_System.Configurations.Banners;

namespace DLyah_Boutique_System.ViewModels;

public class BannerPlacementViewModel {
    public string PageName { get; set; }

    public PageSlot? CurrentPageSlot { get; set; }

    // Lista dos banners que já estão na página
    public List<BannerPlacementModel> CurrentPlacements { get; set; } = new List<BannerPlacementModel>();

    // Lista de banners disponíveis para adicionar
    public List<BannerModel> AvailableBanners { get; set; } = new List<BannerModel>();

    public List<SelectListItem> AvailablePages { get; set; } = new();

    // Propriedades para o formulário de adição
    public int BannerIdToAdd { get; set; }

    public string PositionToAdd { get; set; }

    public string DisplayTypeToAdd { get; set; } = string.Empty;

    public string? LayoutNameToAdd { get; set; }

    public List<SelectListItem> AvailablePositions { get; set; } = new List<SelectListItem>();
}