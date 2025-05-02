using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class ColorController : Controller {
    private readonly IColorRepository _colorRepository;

    public ColorController(IColorRepository colorRepository) {
        _colorRepository = colorRepository;
    }
    
    // GET
    public IActionResult Index() {
        List<ColorModel> colors = _colorRepository.FindAll();
        return View(colors);
    }

    public IActionResult Edit(int id) {
        ColorModel color = _colorRepository.FindById(id);
        return View(color);
    }
    
    public IActionResult Register(int id) {
        return View();
    }
    
    // POST
    [HttpPost]
    public IActionResult Edit(ColorModel c) {
        _colorRepository.Update(c);
        return RedirectToAction("Color");
    }
    
    [HttpPost]
    public IActionResult Register(ColorModel c) {
        if (ModelState.IsValid) {
            _colorRepository.Create(c);
            return RedirectToAction("Color");
        }

        return View(c);
    }
}