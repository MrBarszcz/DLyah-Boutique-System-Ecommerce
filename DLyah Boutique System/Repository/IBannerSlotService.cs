using Microsoft.AspNetCore.Mvc.Rendering;

namespace DLyah_Boutique_System.Repository;

public interface IBannerSlotService {
    List<SelectListItem> FindPages();
    List<SelectListItem> FindPositions(string pageName);
}