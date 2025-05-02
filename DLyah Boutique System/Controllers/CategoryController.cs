using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class CategoryController : Controller {
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository) {
        _categoryRepository = categoryRepository;
    }
    
    // GET
    public IActionResult Index() {
        // lists the categories and has a button that sends to Register, UpdateCategory, DeleteCategory
        List<CategoryModel> categories = _categoryRepository.FindAll();
        return View(categories);
    }
    
    public IActionResult Edit(int id) { // form to update a category
        CategoryModel category = _categoryRepository.FindById(id);
        return View(category);
    }
    
    public IActionResult Register() {
        // form to register a category
        return View();
    }
    
    // POST
    [HttpPost]
    public IActionResult Edit(CategoryModel ct) {
        _categoryRepository.Update(ct);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Register(CategoryModel ct) {
        _categoryRepository.Create(ct);
        return RedirectToAction("Index");
    }
}