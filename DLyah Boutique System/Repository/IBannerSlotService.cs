using DLyah_Boutique_System.Configurations.Banners;
using System.Collections.Generic;

namespace DLyah_Boutique_System.Repository;

public interface IBannerSlotService {
    // Retorna a lista de tipos de página disponíveis (ex: "Home", "Category")
    List<string> FindPages();

    // Retorna TODAS as configurações de uma página específica
    PageSlot FindPositions(string pageType);
}
