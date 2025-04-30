using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface ICategoryRepository {
    List<CategoryModel> FindAll();
    CategoryModel FindById(int id);
    CategoryModel Update(CategoryModel category);
    CategoryModel Create(CategoryModel category);
}