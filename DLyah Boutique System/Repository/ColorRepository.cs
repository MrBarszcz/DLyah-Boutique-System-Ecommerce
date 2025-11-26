using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public class ColorRepository : IColorRepository {
    private readonly BankContext _context;
    
    public ColorRepository(BankContext context) {
        _context = context;
    }
    
    public List<ColorModel> FindAll() {
        return _context.Colors.ToList();
    }

    public ColorModel FindById(int id) {
        return _context.Colors.FirstOrDefault(x => x.ColorId == id);
    }

    public ColorModel FindByName( string name ) {
        return _context.Colors.FirstOrDefault( x => x.Color == name );
    }

    public ColorModel Update(ColorModel color) { 
        ColorModel colorDb = FindById(color.ColorId);

        if (colorDb == null) throw new Exception("Houve um erro ao atualizar a cor");
        
        colorDb.Color = color.Color;
        
        _context.Colors.Update(colorDb);
        _context.SaveChanges();
        
        return colorDb;
    }

    public ColorModel Create(ColorModel color) {
        _context.Colors.Add(color);
        _context.SaveChanges();
        
        return color;
    }
}