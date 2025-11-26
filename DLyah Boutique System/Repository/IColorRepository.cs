using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public interface IColorRepository {
    List<ColorModel> FindAll();
    ColorModel FindById(int id);
    ColorModel FindByName(string name);
    ColorModel Update(ColorModel color);
    ColorModel Create(ColorModel color);
}