using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Repository;

public class BannerRepository : IBannerRepository
{
    private readonly BankContext _context;

    public BannerRepository(BankContext context) {
        _context = context;
    }

    public List<BannerModel> FindByPage(string pageName) {
        return _context.BannerPlacements // 1. Começa pela tabela de "regras"
            .Include(p => p.Banner) // 2. Inclui os dados do banner associado
            .Where(
                p => p.PageName == pageName && p.IsActive && p.Banner.IsActive
            ) // 3. Filtra pela página e se AMBOS (regra e banner) estão ativos
            .OrderBy(p => p.DisplayOrder) // 4. Ordena pela ordem de exibição
            .Select(p => p.Banner) // 5. Seleciona apenas os dados do Banner para retornar
            .ToList();
    }

    public BannerModel FindById(int id) {
        return _context.Banners.FirstOrDefault(b => b.BannerId == id);
    }

    public List<BannerModel> FindAll() {
        return _context.Banners.ToList();
    }

    public BannerModel Update(BannerModel banner) {
        _context.Banners.Update(banner);
        return banner;
    }

    public BannerModel Create(BannerModel banner) {
        _context.Banners.Add(banner);
        return banner;
    }

    public void Delete(int id) {
        var banner = FindById(id);
        if (banner != null) {
            _context.Banners.Remove(banner);
        }
    }

    public Task<int> SaveChanges() {
        return _context.SaveChangesAsync();
    }
}