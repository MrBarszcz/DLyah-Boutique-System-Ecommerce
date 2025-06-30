using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class BannerPlacementController : Controller {
    private readonly IBannerRepository _bannerRepository;
    private readonly IBannerPlacementRepository _placementRepository;
    private readonly IBannerSlotService _slotService;

    public BannerPlacementController(IBannerRepository bannerRepository,
        IBannerPlacementRepository placementRepository, IBannerSlotService slotService) {
        _bannerRepository = bannerRepository;
        _placementRepository = placementRepository;
        _slotService = slotService;
    }

    // GET
    public IActionResult Index() {
        var allPlacements = _placementRepository.FindAll();
        return View(allPlacements);
    }

    public IActionResult Create() {
        var viewModel = new SelectPageViewModel {
            AvailablePages = _slotService.FindPages()
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
        if (string.IsNullOrEmpty(pageName)) {
            return RedirectToAction(nameof(Create));
        }

        // --- LÓGICA DE LISTAGEM CORRIGIDA AQUI ---

        // 1. Busca os banners que já estão na página
        var placementsOnPage = _placementRepository.FindByPage(pageName);
        // Cria um conjunto de IDs dos banners já posicionados para uma busca rápida
        var placedBannerIds = placementsOnPage.Select(p => p.BannerId)
            .ToHashSet();

        // 2. Busca TODOS os banners que estão ATIVOS no catálogo geral
        var allActiveBanners = _bannerRepository.FindAll()
            .Where(b => b.IsActive)
            .ToList();

        // 3. FILTRA: Pega os banners ativos que AINDA NÃO estão na lista de banners já posicionados
        var availableBanners = allActiveBanners.Where(b => !placedBannerIds.Contains(b.BannerId))
            .ToList();

        // 4. Monta a ViewModel com as listas corretas
        var viewModel = new BannerPlacementViewModel {
            PageName = pageName,
            CurrentPlacements = placementsOnPage,
            AvailableBanners = availableBanners, // A lista agora está correta!
            AvailablePositions = _slotService.FindPositions(pageName)
        };
        return View(viewModel);
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> AddPlacement(BannerPlacementViewModel viewModel) {
        if (viewModel.BannerIdToAdd > 0 && !string.IsNullOrEmpty(viewModel.PositionToAdd)) {
            _placementRepository.AddPlacement(viewModel.PageName, viewModel.BannerIdToAdd, viewModel.PositionToAdd);
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