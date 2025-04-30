using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DLyah_Boutique_System.Repository;

public class CategoryRepository : ICategoryRepository {
    private readonly BankContext _context;
    
    public CategoryRepository(BankContext context) {
        _context = context;
    }

    // FIND
    public List<CategoryModel> FindAll() {
        return _context.Categories.ToList();
    }

    public CategoryModel FindById(int id) {
        return _context.Categories.FirstOrDefault(x => x.CategoryId == id);
    }

    // UPDATE
    public CategoryModel Update(CategoryModel category) {
        CategoryModel categoryDb = FindById(category.CategoryId);

        if (categoryDb == null) throw new Exception("Houve um erro ao atualizar a categoria");
        
        categoryDb.Category = category.Category;
        
        _context.Categories.Update(categoryDb);
        _context.SaveChanges();
        
        return categoryDb;
    }
    
    // CREATE
    public CategoryModel Create(CategoryModel category) {
        _context.Categories.Add(category);
        _context.SaveChanges();

        return category;
    }
    
    // KILL
    
}