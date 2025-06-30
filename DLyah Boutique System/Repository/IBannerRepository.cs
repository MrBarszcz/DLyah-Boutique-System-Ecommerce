using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IBannerRepository {
    List<BannerModel> FindByPage(string pageName);
    
    BannerModel FindById(int id);
    List<BannerModel> FindAll();
    BannerModel Update(BannerModel banner);
    BannerModel Create(BannerModel banner);
    void Delete(int id);
    
    Task<int> SaveChanges();
}