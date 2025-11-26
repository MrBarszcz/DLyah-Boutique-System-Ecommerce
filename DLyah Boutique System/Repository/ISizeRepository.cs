using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface ISizeRepository {
    List<SizeModel> FindAll();
    SizeModel FindById(int id);
    SizeModel FindByName(string name);
    SizeModel Update(SizeModel size);
    SizeModel Add(SizeModel size);
    
    Task<int> SaveChanges();
}