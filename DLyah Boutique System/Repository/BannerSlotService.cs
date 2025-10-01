using DLyah_Boutique_System.Configurations.Banners;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace DLyah_Boutique_System.Repository;

public class BannerSlotService : IBannerSlotService {
    private readonly BannerSlotSettings _slotSettings;

    // Injetamos as configurações que foram carregadas no Program.cs
    public BannerSlotService(IOptions<BannerSlotSettings> slotSettings) {
        // .Value acessa o objeto BannerSlotSettings preenchido com os dados do JSON
        _slotSettings = slotSettings.Value;
    }

    public List<string> FindPages() {
        // Lê os tipos de página diretamente do objeto de configuração
        return _slotSettings.PageSlots?.Select(p => p.PageType).ToList() ?? new List<string>();
    }

    public PageSlot FindPositions(string pageType) {
        // Encontra e retorna o objeto de configuração completo para a página solicitada
        return _slotSettings.PageSlots?.FirstOrDefault(p => p.PageType == pageType);
    }
}
