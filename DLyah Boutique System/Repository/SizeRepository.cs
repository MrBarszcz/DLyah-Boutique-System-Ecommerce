using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public class SizeRepository : ISizeRepository {
    private readonly BankContext _context;
    
    public SizeRepository(BankContext context) {
        _context = context;
    }
    
    public List<SizeModel> FindAll() {
        return _context.Sizes.ToList();
    }

    public SizeModel FindById(int id) {
        return _context.Sizes.FirstOrDefault(x => x.SizeId == id);
    }

    public SizeModel FindByName( string name ) {
        return _context.Sizes.FirstOrDefault(x => x.Size == name);
    }

    public SizeModel Update(SizeModel size) { 
        SizeModel sizeDb = FindById(size.SizeId);

        if (sizeDb == null) throw new Exception("Houve um erro ao atualizar o tamanho");
        
        sizeDb.Size = size.Size;
        
        _context.Sizes.Update(sizeDb);
        _context.SaveChanges();
        
        return sizeDb;
    }

    public SizeModel Add(SizeModel size) {
        _context.Sizes.Add(size);
        
        return size;
    }

    public async Task<int> SaveChanges() {
        return await _context.SaveChangesAsync();
    }
}