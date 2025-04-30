using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IProductRepository {
    List<ProductModel> FindAll();
    ProductModel? FindById(int id);

    ProductModel Update(ProductModel product);

    ProductModel Create(ProductModel product);
    
    ProductModel Kill(ProductModel product);
}