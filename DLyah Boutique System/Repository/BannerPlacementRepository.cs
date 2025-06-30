using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Repository;

public class BannerPlacementRepository : IBannerPlacementRepository {
    private readonly BankContext _context;

    public BannerPlacementRepository(BankContext context) {
        _context = context;
    }

    public List<BannerPlacementModel> FindByPage(string pageName) {
        return _context.BannerPlacements
            .Include(p => p.Banner) // Inclui os dados do banner
            .Where(p => p.PageName == pageName)
            .OrderBy(p => p.DisplayOrder)
            .ToList();
    }

    public async Task<BannerPlacementModel> FindById(int placementId) {
        return await _context.BannerPlacements.FindAsync(placementId);
    }

    public void AddPlacement(string pageName, int bannerId, string position) {
        // Calcula a próxima ordem de exibição para esta página e posição específicas.
        var lastOrder = _context.BannerPlacements
                            .Where(p => p.PageName == pageName && p.Position == position)
                            .Max(p => (int?)p.DisplayOrder)
                        ?? 0;

        var newPlacement = new BannerPlacementModel {
            PageName = pageName,
            BannerId = bannerId,
            Position = position, // Salva a posição
            DisplayOrder = lastOrder + 1, // Define como o próximo na ordem
            IsActive = true
        };

        _context.BannerPlacements.Add(newPlacement);
    }

    public List<BannerPlacementModel> FindAll() {
        return _context.BannerPlacements
            .Include(p => p.Banner) // Inclui os dados do banner para exibir o título na lista
            .OrderBy(p => p.PageName)
            .ThenBy(p => p.DisplayOrder)
            .ToList();
    }

    public void UpdatePlacement(int placementId, string pageName, int bannerId) {
        throw new NotImplementedException();
    }

    public void Delete(int placementId) {
        var placement = _context.BannerPlacements.Find(placementId);
        if (placement != null) {
            _context.BannerPlacements.Remove(placement);
        }
    }

    public Task<int> SaveChanges() {
        return _context.SaveChangesAsync();
    }
}