using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class ProductController : Controller {
    private readonly IProductRepository _productRepository;
    private readonly IGenderRepository _genderRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private readonly ISizeRepository _sizeRepository;

    public ProductController(IProductRepository productRepository, IGenderRepository genderRepository, ICategoryRepository categoryRepository, IColorRepository colorRepository, ISizeRepository sizeRepository) {
        _productRepository = productRepository;
        _genderRepository = genderRepository;
        _categoryRepository = categoryRepository;
        _colorRepository = colorRepository;
        _sizeRepository = sizeRepository;
    }
    // GET
    public IActionResult Index() {
        return View();
    }

    public IActionResult Register() {
        List<GenderModel> genders = _genderRepository.FindAll();
        List<CategoryModel> categories = _categoryRepository.FindAll();
        List<ColorModel> colors = _colorRepository.FindAll();
        List<SizeModel> sizes = _sizeRepository.FindAll();
        
        ViewBag.Genders = genders;
        ViewBag.Categories = categories;
        ViewBag.Colors = colors;
        ViewBag.Sizes = sizes;
        
        return View();
    }
}