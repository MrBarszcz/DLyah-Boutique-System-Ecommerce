using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.Repository;

public class GenderRepository : IGenderRepository {
    private readonly BankContext _context;
    
    public GenderRepository(BankContext context) {
        _context = context;
    }
    
    public List<GenderModel> FindAll() {
        return _context.Genders.ToList();
    }
}