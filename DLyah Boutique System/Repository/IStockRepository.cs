using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IStockRepository {
    StockProductModel? FindById(int productId, int colorId, int sizeId);
    
    StockProductModel Update(StockProductModel stock);
    
    StockProductModel Create(StockProductModel stock);
    
    StockProductModel Kill(StockProductModel stock);
}