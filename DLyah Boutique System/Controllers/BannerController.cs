using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.ViewModels;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class BannerController : Controller {
    private readonly IBannerRepository _bannerRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BannerController(IBannerRepository bannerRepository, IWebHostEnvironment webHostEnvironment) {
        _bannerRepository = bannerRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET
    public IActionResult Index() {
        var banners = _bannerRepository.FindAll();
        return View(banners);
    }

    public IActionResult Register() {
        return View();
    }

    public IActionResult Edit(int id) {
        var banner = _bannerRepository.FindById(id);
        if (banner == null) return NotFound();

        var viewModel = new BannerViewModel {
            BannerId = banner.BannerId,
            Title = banner.Title,
            Description = banner.Description,
            LinkUrl = banner.LinkUrl,
            ExistingImageUrl = banner.ImageUrl,
            IsActive = banner.IsActive
        };

        return View(viewModel);
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Register(BannerViewModel viewModel) {
        if (viewModel.ImageUpload == null || viewModel.ImageUpload.Length == 0) {
            ModelState.AddModelError("ImageUpload", "Por favor, selecione uma imagem para o banner.");
        }

        if (ModelState.IsValid) {
            string uniqueFileName = await UploadImage(viewModel.ImageUpload);

            var banner = new BannerModel {
                Title = viewModel.Title,
                Description = viewModel.Description,
                LinkUrl = viewModel.LinkUrl,
                ImageUrl = $"/images/banners/{uniqueFileName}", // Salva o caminho relativo
                IsActive = viewModel.IsActive
            };

            _bannerRepository.Create(banner);
            await _bannerRepository.SaveChanges();

            TempData["SuccessMessage"] = "Banner criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BannerViewModel viewModel)
    {
        if (id != viewModel.BannerId) return BadRequest();

        if (ModelState.IsValid)
        {
            var bannerToUpdate = _bannerRepository.FindById(id);
            if (bannerToUpdate == null) return NotFound();

            // Se uma nova imagem foi enviada, faz o upload e atualiza o caminho
            if (viewModel.ImageUpload != null)
            {
                // Opcional: deletar a imagem antiga do servidor
                if (!string.IsNullOrEmpty(bannerToUpdate.ImageUrl))
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, bannerToUpdate.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                string uniqueFileName = await UploadImage(viewModel.ImageUpload);
                bannerToUpdate.ImageUrl = $"/images/banners/{uniqueFileName}";
            }

            bannerToUpdate.Title = viewModel.Title;
            bannerToUpdate.Description = viewModel.Description;
            bannerToUpdate.LinkUrl = viewModel.LinkUrl;
            bannerToUpdate.IsActive = viewModel.IsActive;
                
            _bannerRepository.Update(bannerToUpdate);
            await _bannerRepository.SaveChanges();
                
            TempData["SuccessMessage"] = "Banner atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    [ HttpPost ] // Garante que só pode ser acessada via POST
    [ ValidateAntiForgeryToken ] // Protege contra ataques CSRF
    public async Task<IActionResult> Delete(int id) {
        var banner = _bannerRepository.FindById(id);
        if (banner == null) {
            return NotFound(
                new {
                    success = false,
                    message = "Banner não encontrado."
                }
            );
        }

        try {
            // Deleta a imagem física do servidor
            if (!string.IsNullOrEmpty(banner.ImageUrl)) {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, banner.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPath)) {
                    System.IO.File.Delete(fullPath);
                }
            }

            _bannerRepository.Delete(id);
            await _bannerRepository.SaveChanges();

            // Retorna uma resposta de sucesso em formato JSON
            return Ok(
                new {
                    success = true,
                    message = "Banner excluído com sucesso!"
                }
            );
        } catch (Exception ex) {
            // Em caso de erro, retorna um status 500 com uma mensagem
            return StatusCode(
                500,
                new {
                    success = false,
                    message = ex.Message
                }
            );
        }
    }

    private async Task<string> UploadImage(IFormFile imageFile) {
        string uniqueFileName = $"{Guid.NewGuid().ToString().Substring(0, 8)}-{Path.GetFileName(imageFile.FileName)}";
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        if (!Directory.Exists(uploadsFolder)) {
            Directory.CreateDirectory(uploadsFolder);
        }

        await using (var fileStream = new FileStream(filePath, FileMode.Create)) {
            await imageFile.CopyToAsync(fileStream);
        }

        return uniqueFileName;
    }
}