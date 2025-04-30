using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class ControlPanelController : Controller {
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IColorRepository _colorRepository;

    private readonly ILogger<ControlPanelController> _logger;

    public ControlPanelController(
        ICategoryRepository categoryRepository, ISizeRepository sizeRepository, IColorRepository colorRepository,
        ILogger<ControlPanelController> logger
    ) {
        _categoryRepository = categoryRepository;
        _sizeRepository = sizeRepository;
        _colorRepository = colorRepository;

        _logger = logger;
    }

    // GET
    public IActionResult Index() { // briefly list the requests
        return View();
    }

    public IActionResult Category() {
        // lists the categories and has a button that sends to RegisterCategory, UpdateCategory, DeleteCategory
        List<CategoryModel> categories = _categoryRepository.FindAll();
        return View(categories);
    }

    public IActionResult Size() {
        List<SizeModel> sizes = _sizeRepository.FindAll();
        return View(sizes);
    }

    public IActionResult Color() {
        List<ColorModel> colors = _colorRepository.FindAll();
        return View(colors);
    }

    public IActionResult EditCategory(int id) { // form to update a category
        CategoryModel category = _categoryRepository.FindById(id);
        return View(category);
    }
    
    public IActionResult EditSize(int id) {
        SizeModel size = _sizeRepository.FindById(id);
        return View(size);
    }

    public IActionResult EditColor(int id) {
        ColorModel color = _colorRepository.FindById(id);
        return View(color);
    }

    public IActionResult RegisterCategory() { // form to register a new category
        return View();
    }

    public IActionResult RegisterSize() {
        return View();
    }

    public IActionResult RegisterColor(int id) {
        return View();
    }

    // public IActionResult KillCategory(int id) {
    //     return PartialView("_KillCategory", id);
    // }

    // POST
    [HttpPost]
    public IActionResult EditCategory(CategoryModel ct) {
        _categoryRepository.Update(ct);
        return RedirectToAction("Category");
    }

    [HttpPost]
    public IActionResult EditSize(SizeModel s) {
        _sizeRepository.Update(s);
        return RedirectToAction("Size");
    }
    
    [HttpPost]
    public IActionResult EditColor(ColorModel c) {
        _colorRepository.Update(c);
        return RedirectToAction("Color");
    }

    [HttpPost]
    public IActionResult RegisterCategory(CategoryModel ct) {
        _categoryRepository.Create(ct);
        return RedirectToAction("Category");
    }

    [HttpPost]
    public IActionResult RegisterSize(SizeModel s) {
        _sizeRepository.Create(s);
        return RedirectToAction("Size");
    }

    [HttpPost]
    public IActionResult RegisterColor(ColorModel c) {
        if (ModelState.IsValid) {
            _colorRepository.Create(c);
            return RedirectToAction("Color");
        }

        return View(c);
    }
}