using DLyah_Boutique_System.Models;

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
    ProductModel Kill(ProductModel product);
    
    Task<int> SaveChanges();
}