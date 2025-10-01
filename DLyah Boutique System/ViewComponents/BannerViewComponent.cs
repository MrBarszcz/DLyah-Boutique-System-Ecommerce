using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.ViewComponents;

public class BannerViewComponent : ViewComponent {
    private readonly IBannerPlacementRepository _placementRepository;

    public BannerViewComponent( IBannerPlacementRepository placementRepository ) {
        _placementRepository = placementRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync( string pageName, string position ) {
        // Busca todos os banners para esta página e posição
        var placements = await this._placementRepository.FindByPageAndPositionAsync( pageName, position );

        if (!placements.Any()) {
            // Se não houver banners, não renderiza nada
            return Content( string.Empty );
        }

        // Pega o tipo de exibição do primeiro banner
        // (Assumimos que todos na mesma posição usam o mesmo tipo)
        var displayType = placements.First().DisplayType;

        // Retorna a View com o nome correspondente ao DisplayType (ex: "Carousel", "Mosaic")
        // O sistema procurará por Carousel.cshtml, Mosaic.cshtml, etc.
        return View( displayType, placements );
    }
}