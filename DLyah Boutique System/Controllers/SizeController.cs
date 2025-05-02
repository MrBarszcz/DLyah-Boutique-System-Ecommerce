using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class SizeController : Controller {
    private readonly ISizeRepository _sizeRepository;

    public SizeController(ISizeRepository sizeRepository) {
        _sizeRepository = sizeRepository;
    }
    
    // GET
    public IActionResult Index() {
        List<SizeModel> sizes = _sizeRepository.FindAll();
        return View(sizes);
    }
    
    public IActionResult Edit(int id) {
        SizeModel size = _sizeRepository.FindById(id);
        return View(size);
    }
    
    public IActionResult Register() {
        return View();
    }
    
    // POST
    [HttpPost]
    public IActionResult Edit(SizeModel s) {
        _sizeRepository.Update(s);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Register(SizeModel s) {
        _sizeRepository.Create(s);
        return RedirectToAction("Index");
    }
}