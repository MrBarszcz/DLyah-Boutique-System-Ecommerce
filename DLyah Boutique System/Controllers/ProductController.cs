using System.Net.Http.Headers;
using System.Text.Json;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DLyah_Boutique_System.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IGenderRepository _genderRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ProductController> _logger;

    // private readonly string _pathImage;

    public ProductController(
        IProductRepository productRepository, IGenderRepository genderRepository,
        ICategoryRepository categoryRepository, IColorRepository colorRepository, ISizeRepository sizeRepository,
        IWebHostEnvironment environment, IFileUploadService fileUploadService, ILogger<ProductController> logger
    ) {
        _productRepository = productRepository;
        _genderRepository = genderRepository;
        _categoryRepository = categoryRepository;
        _colorRepository = colorRepository;
        _sizeRepository = sizeRepository;
        _environment = environment;
        _logger = logger;
        // _pathImage = Path.Combine(_environment.WebRootPath, "images", "products");
    }

    // GET
    public IActionResult Index() {
        return View();
    }

    public IActionResult Register() {
        var viewModel = new ProductRegisterViewModel {
            AvailableGenders = _genderRepository.FindAll(),
            AvailableCategories = _categoryRepository.FindAll(),
            AvailableColors = _colorRepository.FindAll(),
            AvailableSizes = _sizeRepository.FindAll()
        };
        return View(viewModel);
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Register(ProductRegisterViewModel viewModel) {
        // Garante que a lista de Stock não seja nula para evitar erros de referência
        if (viewModel.Stock == null) viewModel.Stock = new List<StockProductModel>();

        if (viewModel.PImages == null) viewModel.PImages = new List<IFormFile>();
        
        if (viewModel.SelectedCategories == null) viewModel.SelectedCategories = new List<int>();
        
        if (viewModel.SelectedColors == null) viewModel.SelectedColors = new List<int>();
        
        if (viewModel.SelectedSizes == null) viewModel.SelectedSizes = new List<int>();
        
        bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        if (ModelState.IsValid) {
            try {
                var product = new ProductModel {
                    ProductName = viewModel.ProductName!,
                    ProductDescription = viewModel.ProductDescription!,
                    ProductPrice = viewModel.ProductPrice,
                    GenderId = viewModel.GenderId,
                    ProductQuantity = viewModel.Stock?.Sum(s => s.StockQuantity) ?? 0
                };

                // Adiciona o produto ao contexto do repositório (sem salvar ainda, se o Create não salva)
                _productRepository.Create(product);
                // Salva o produto principal para obter o ProductId gerado pelo banco
                await _productRepository.SaveChanges();

                // 2. Processar e salvar Imagens do Produto
                if (viewModel.PImages.Any()) {
                    string uploadDir = Path.Combine(_environment.WebRootPath, "images", "products");
                    
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    int order = 1;
                    foreach (var imageFile in viewModel.PImages) {
                        if (imageFile.Length > 0) {
                            string uniqueFileName =
                                $"{product.ProductName?.Replace(" ", "-").ToLower()}-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(imageFile.FileName)}";
                            string filePath = Path.Combine(uploadDir, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create)) {
                                await imageFile.CopyToAsync(stream);
                            }
                            
                            _productRepository.CreateProductImage(
                                new ProductImageModel {
                                    ProductId = product.ProductId,
                                    ProductImagePath =
                                        $"/images/products/{uniqueFileName}", // Caminho relativo para a view
                                    ImageOrder = order++
                                }
                            );
                        }
                    }
                }

                // 3. Associar Categorias selecionadas
                if (viewModel.SelectedCategories != null && viewModel.SelectedCategories.Any()) {
                    foreach (var categoryId in viewModel.SelectedCategories) {
                        _productRepository.CreateProductCategory(product.ProductId, categoryId);
                    }
                }

                // 4. Associar Cores selecionadas
                if (viewModel.SelectedColors != null && viewModel.SelectedColors.Any()) {
                    foreach (var colorId in viewModel.SelectedColors) {
                        _productRepository.CreateProductColor(product.ProductId, colorId);
                    }
                }

                // 5. Associar Tamanhos selecionados
                if (viewModel.SelectedSizes != null && viewModel.SelectedSizes.Any()) {
                    foreach (var sizeId in viewModel.SelectedSizes) {
                        _productRepository.CreateProductSize(product.ProductId, sizeId);
                    }
                }

                // 6. Criar entradas de Estoque
                if (viewModel.Stock.Any()) {
                    foreach (var stockInput in viewModel.Stock) {
                        var stockItem = new StockProductModel {
                            ProductId = product.ProductId,
                            ColorId = stockInput.ColorId,
                            SizeId = stockInput.SizeId,
                            StockQuantity = stockInput.StockQuantity
                        };
                        
                        _productRepository.CreateStockProduct(stockItem);
                    }
                }

                // Salva todas as entidades relacionadas (imagens, categorias, cores, tamanhos, estoque)
                await _productRepository.SaveChanges();

                // TempData["SuccessMessage"] = "Produto cadastrado com sucesso!"; // Opcional: mensagem de sucesso
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                Console.WriteLine(
                    $"{ex} Erro ao salvar o produto. ViewModel: {System.Text.Json.JsonSerializer.Serialize(viewModel)}"
                );
                ModelState.AddModelError("", "Ocorreu um erro inesperado ao salvar o produto. Tente novamente.");
                // Não se esqueça de repopular os dados para os dropdowns/listas se houver um erro aqui
            }
        } else {
            foreach (var state in ModelState) {
                if (state.Value.Errors.Any()) {
                    Console.WriteLine(
                        $"Campo: {state.Key}, Erros: {string.Join("; ", state.Value.Errors.Select(e => e.ErrorMessage))}"
                    );
                }

                if (isAjaxRequest) {
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    
                    return BadRequest(new {sucess = false, errors = errors, message = "Dados Invalidos"});
                }
            }
        }

        // Se ModelState inválido ou ocorreu um erro, recarregue os dados para os dropdowns/listas
        viewModel.AvailableGenders = _genderRepository.FindAll();
        viewModel.AvailableCategories = _categoryRepository.FindAll();
        viewModel.AvailableColors = _colorRepository.FindAll();
        viewModel.AvailableSizes = _sizeRepository.FindAll();
        return View(viewModel);
    }
}