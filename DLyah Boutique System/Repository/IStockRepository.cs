using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IStockRepository {
    StockProductModel? FindById(int productId, int colorId, int sizeId);
    StockProductModel? FindProduct(int productId);
    StockProductModel? FindColor(int colorId);
    StockProductModel? FindSize(int sizeId);
    
    StockProductModel Update(StockProductModel stock);
    
    StockProductModel Create(StockProductModel stock);
    
    StockProductModel Kill(StockProductModel stock);
}