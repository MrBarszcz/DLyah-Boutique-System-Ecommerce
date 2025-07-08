using Microsoft.AspNetCore.Mvc.Rendering;

namespace DLyah_Boutique_System.Repository;

public class BannerSlotService : IBannerSlotService {
    private readonly ICategoryRepository _categoryRepo;

    // O mapa central de todas as posições de banner do site.
    // A chave é o "Tipo de Página", o valor é uma lista de nomes de posição.
    private readonly Dictionary<string, List<string>> _slotMap = new Dictionary<string, List<string>> {
        {
            "Home", new List<string> {
                "Header",
                "Body-Top",
                "Body-Bottom"
            }
        }
        // Adicione mais tipos de página e suas posições aqui
    };

    public BannerSlotService(ICategoryRepository categoryRepo) {
        _categoryRepo = categoryRepo;
    }

    public List<SelectListItem> FindPages() {
        var pages = new List<SelectListItem> {
            new SelectListItem {
                Value = "Home",
                Text = "Página Inicial (Homepage)"
            }
        };

        var allCategories = _categoryRepo.FindAll();
        foreach (var category in allCategories) {
            // A página de categoria usará as posições definidas para o tipo "Category" no mapa.
            pages.Add(
                new SelectListItem {
                    Value = $"Category_{category.CategoryId}",
                    Text = $"Página da Categoria: {category.Category}"
                }
            );
        }

        return pages;
    }

    public List<SelectListItem> FindPositions(string pageName) {
        string pageType = pageName.Contains('_') ? pageName.Split('_')[0] : pageName;

        if (_slotMap.ContainsKey(pageType)) {
            return _slotMap[pageType]
                .Select(
                    pos => new SelectListItem {
                        Value = pos,
                        Text = pos
                    }
                )
                .ToList();
        }

        // Retorna uma lista vazia se a página não tiver posições definidas
        return new List<SelectListItem>();
    }
}