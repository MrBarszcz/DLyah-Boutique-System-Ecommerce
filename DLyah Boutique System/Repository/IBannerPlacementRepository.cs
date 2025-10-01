using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IBannerPlacementRepository {
    List<BannerPlacementModel> FindByPage(string pageName);
    Task<BannerPlacementModel> FindById(int placementId);
    List<BannerPlacementModel> FindAll();
    Task<List<BannerPlacementModel>> FindByPageAndPositionAsync(string pageName, string position);

    void AddPlacement( string pageName, int bannerId, string position, string displayType, string? layoutName );
    void UpdatePlacement(int placementId, string pageName, int bannerId);
    void Delete(int placementId);
    Task<int> SaveChanges();
}