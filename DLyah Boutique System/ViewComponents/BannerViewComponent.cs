using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.ViewComponents;

public class BannerViewComponent : ViewComponent {
    private readonly IBannerPlacementRepository _placementRepository;

    public BannerViewComponent(IBannerPlacementRepository placementRepository) {
        _placementRepository = placementRepository;
    }

    public IViewComponentResult Invoke(string pageName, string position) {
        // ... sua lógica para buscar os banners ...
        var placements = _placementRepository.FindByPage(pageName)
            .Where(p => p.Position == position)
            .ToList();

        return View(placements);
    }
}