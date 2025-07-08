using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.ViewModels;

namespace DLyah_Boutique_System.Repository;

public interface IProductRepository {
    List<ProductModel> FindAll();
    ProductModel? FindById(int id);

    ProductModel Update(ProductModel product);

    ProductModel Create(ProductModel product);
    ProductCategoryModel CreateProductCategory(int productId, int categoryId);
    ProductColorModel CreateProductColor(int productId, int colorId);
    ProductSizeModel CreateProductSize(int productId, int sizeId);
    ProductImageModel CreateProductImage(ProductImageModel productImage);
    StockProductModel CreateStockProduct(StockProductModel stock);
    void Kill(int id);
    
    void UpdateProductCategories(int productId, List<int> categoryIds);
    void UpdateProductColors(int productId, List<int> colorIds);
    void UpdateProductSizes(int productId, List<int> sizeIds);
    void DeleteImages(List<int> imageIds, string webRootPath);
    
    void UpdateStock(int productId, List<StockEditViewModel> stockEntries);
    
    Task<int> SaveChanges();
}