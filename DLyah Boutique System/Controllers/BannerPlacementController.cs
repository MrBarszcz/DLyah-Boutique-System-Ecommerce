using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DLyah_Boutique_System.Configurations.Banners;

namespace DLyah_Boutique_System.Controllers;

public class BannerPlacementController : Controller {
    private readonly IBannerRepository _bannerRepository;
    private readonly IBannerPlacementRepository _placementRepository;
    private readonly IBannerSlotService _slotService;
    private readonly ICategoryRepository _categoryRepository;
    public BannerPlacementController(
        IBannerRepository bannerRepository,
        IBannerPlacementRepository placementRepository,
        IBannerSlotService slotService,
        ICategoryRepository categoryRepository
        ) {
        _bannerRepository = bannerRepository;
        _placementRepository = placementRepository;
        _slotService = slotService;
        _categoryRepository = categoryRepository;
    }

    // GET
    public IActionResult Index() {
        var allPlacements = _placementRepository.FindAll();
        return View(allPlacements);
    }

    public IActionResult Create() {
        // 1. Obtenha a lista de strings do serviço (Ex: ["Home", "Category"])
        var pageTypes = _slotService.FindPages();

        // 2. Converta a List<string> para List<SelectListItem>
        var pagesForDropdown = pageTypes.Select(p => new SelectListItem {
            Value = p, // O valor que será enviado pelo formulário
            Text = p   // O texto que o usuário verá no dropdown
        }).ToList();

        // 3. Crie o ViewModel com a lista já convertida
        var viewModel = new SelectPageViewModel {
            AvailablePages = pagesForDropdown
        };
        
        return View(viewModel);
    }

    public IActionResult ManagePage(string selectedPageName) {
        if (string.IsNullOrEmpty(selectedPageName)) {
            // Se nenhum valor for passado, redireciona de volta para a seleção
            return RedirectToAction(nameof(Create));
        }

        // Redireciona para a action 'Manage' com o nome da página correta
        return RedirectToAction(
            nameof(Manage),
            new {
                pageName = selectedPageName
            }
        );
    }

    public IActionResult Manage(string pageName) {
        // A validação inicial está perfeita.
        if (string.IsNullOrEmpty(pageName)) {
            // Se nenhuma página for especificada, talvez seja melhor redirecionar para uma página de seleção?
            // Ou carregar uma página padrão como "Home".
            pageName = "Home"; 
        }

        // 1. Extrai o "tipo de página" do nome da página. Ex: "Category_1" vira "Category"
        string pageType = pageName.Contains('_') ? pageName.Split('_')[0] : pageName;

        // 2. Busca a configuração completa usando o NOVO serviço e o pageType
        var pageSlotConfig = _slotService.FindPositions(pageType);
        var availablePages = _slotService.FindPages();

        // 3. A lógica para buscar banners disponíveis continua perfeita
        var placementsOnPage = _placementRepository.FindByPage(pageName);
        var placedBannerIds = placementsOnPage.Select(p => p.BannerId).ToHashSet();
        var allActiveBanners = _bannerRepository.FindAll().Where(b => b.IsActive).ToList();
        var availableBanners = allActiveBanners.Where(b => !placedBannerIds.Contains(b.BannerId)).ToList();

        // 4. Monta o ViewModel usando SOMENTE os dados do novo serviço
        var viewModel = new BannerPlacementViewModel {
            PageName = pageName,
            CurrentPlacements = placementsOnPage,
            AvailableBanners = availableBanners,
            
            // CORREÇÃO: Cria a lista de posições a partir do pageSlotConfig
            AvailablePositions = pageSlotConfig?.Positions
                .Select(p => new SelectListItem { Value = p.Name, Text = $"{p.Name} ({p.Description})" })
                .ToList() ?? new List<SelectListItem>(), // Garante que a lista nunca seja nula

            AvailablePages = availablePages.Select(p => new SelectListItem { Value = p, Text = p }).ToList(),
            
            // Passa o objeto de configuração completo para a View, como você já estava fazendo
            CurrentPageSlot = pageSlotConfig 
        };

        return View(viewModel);
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> AddPlacement(BannerPlacementViewModel viewModel) {
        if (viewModel.BannerIdToAdd > 0 && !string.IsNullOrEmpty(viewModel.PositionToAdd)) {
            _placementRepository.AddPlacement(
                viewModel.PageName, 
                viewModel.BannerIdToAdd, 
                viewModel.PositionToAdd,
                viewModel.DisplayTypeToAdd,
                viewModel.LayoutNameToAdd 
                );
            await _placementRepository.SaveChanges();
            TempData["SuccessMessage"] = "Banner adicionado à página com sucesso!";
        }

        // Redireciona de volta para a página de gerenciamento correta
        return RedirectToAction(
            nameof(Manage),
            new {
                pageName = viewModel.PageName
            }
        );
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> RemovePlacement(int placementId) {
        var placement = await _placementRepository.FindById(placementId);
        if (placement != null) {
            string pageName = placement.PageName;
            _placementRepository.Delete(placementId);
            await _placementRepository.SaveChanges();
            TempData["SuccessMessage"] = "Banner removido da página com sucesso!";
            return RedirectToAction(
                nameof(Manage),
                new {
                    pageName = pageName
                }
            );
        }

        return NotFound();
    }
}