using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface ICategoryRepository {
    List<CategoryModel> FindAll();
    CategoryModel FindById(int id);
    CategoryModel FindByName(string name);
    CategoryModel Update(CategoryModel category);
    CategoryModel Add(CategoryModel category);
    
    Task<int> SaveChanges(); 
}