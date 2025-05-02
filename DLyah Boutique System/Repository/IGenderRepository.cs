using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IGenderRepository {
    List<GenderModel> FindAll();
}