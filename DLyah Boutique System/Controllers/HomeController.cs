using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;

namespace DLyah_Boutique_System.Controllers;

public class HomeController : Controller {
    private readonly IProductRepository _productRepository;
    private readonly IBannerRepository _bannerRepository;

    public HomeController(IProductRepository productRepository, IBannerRepository bannerRepository) {
        _productRepository = productRepository;
        _bannerRepository = bannerRepository;
    }
    public IActionResult Index() {
        // 2. Busca os produtos no banco. Usamos o FindAll() que já inclui as imagens.
        var allProducts = _productRepository.FindAll();
            
        // 3. Monta a ViewModel com todos os dados para a homepage
        var viewModel = new HomeViewModel{
            // Busca os banners ativos configurados para a "Home" na posição "Header"
            TopBanners = _bannerRepository.FindByPage("Home"),

            // Para a vitrine, vamos pegar os 8 produtos mais recentes que tenham imagem
            ShowcaseProducts = allProducts
                .Where(p => p.ProductImages.Any())
                .OrderByDescending(p => p.ProductId)
                .Take(8)
                .ToList()
        };

        // 4. Envia a ViewModel preenchida para a View
        return View(viewModel);
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}