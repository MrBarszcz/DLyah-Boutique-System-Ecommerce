using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public class StockRepository : IStockRepository {
    private readonly BankContext _context;
    
    public StockRepository(BankContext context) {
        _context = context;
    }
    
    public StockProductModel? FindById(int productId, int colorId, int sizeId) {
        return _context.StockProducts.FirstOrDefault(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId);
    }

    public StockProductModel Update(StockProductModel stock) {
        _context.StockProducts.Update(stock);
        _context.SaveChanges();

        return stock;
    }

    public StockProductModel Create(StockProductModel stock) {
        throw new NotImplementedException();
    }

    public StockProductModel Kill(StockProductModel stock) {
        throw new NotImplementedException();
    }
}