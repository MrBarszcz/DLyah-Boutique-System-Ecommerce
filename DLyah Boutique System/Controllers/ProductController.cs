using System.Net.Http.Headers;
using System.Text.Json;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DLyah_Boutique_System.Controllers;

public class ProductController : Controller {
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
        try {
            // Busca todos os produtos e seus dados relacionados do repositório.
            var productModels = _productRepository.FindAll();

            // Mapeia a lista de ProductModel para a lista de ProductListViewModel.
            var productListViewModels = productModels.Select(
                    product => new ProductListViewModel {
                        Product = product,
                        Gender = product.Gender,
                        Categories = product.ProductCategories
                            .Select(pc => pc.Category)
                            .ToList(),
                        Colors = product.ProductColors
                            .Select(pc => pc.Color)
                            .ToList(),
                        Sizes = product.ProductSizes
                            .Select(ps => ps.Size)
                            .ToList(),
                        Images = product.ProductImages
                            .OrderBy(pi => pi.ImageOrder)
                            .ToList(),
                        StockProducts = product.StockProducts.ToList()
                    }
                )
                .ToList();

            // Envia a lista de ViewModels para a View.
            return View(productListViewModels);
        } catch (Exception ex) {
            _logger.LogError(ex, "Ocorreu um erro ao buscar a lista de produtos.");
            // Retorna uma view de erro ou a mesma view com uma mensagem de erro
            TempData["ErrorMessage"] = "Não foi possível carregar a lista de produtos.";
            return View(new List<ProductListViewModel>()); // Retorna uma lista vazia
        }
    }

    public IActionResult Details(int id) {
        var product = _productRepository.FindById(id);

        if (product == null) {
            return NotFound();
        }

        var stockData = product.StockProducts
            .ToDictionary(
                key => $"{key.ColorId}-{key.SizeId}",
                value => value.StockQuantity
            );

        var viewModel = new ProductDetailViewModel {
            Product = product,
            Gender = product.Gender,
            Images = product.ProductImages
                .OrderBy(i => i.ImageOrder)
                .ToList(),
            Categories = product.ProductCategories
                .Select(pc => pc.Category)
                .ToList(),
            AvailableColors = product.ProductColors
                .Select(pc => pc.Color)
                .ToList(),
            AvailableSizes = product.ProductSizes
                .Select(ps => ps.Size)
                .ToList(),
            StockPerCombination = stockData
        };

        return View(viewModel);
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


    public IActionResult Edit(int id) {
        // Busca o produto no banco, incluindo todos os dados relacionados
        var product = _productRepository.FindById(id);
        if (product == null) {
            return NotFound(); // Retorna erro 404 se o produto não existir
        }

        // Mapeia os dados do modelo de domínio para a ViewModel de Edição
        var viewModel = new ProductEditViewModel {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductDescription = product.ProductDescription,
            ProductPrice = product.ProductPrice,
            GenderId = product.GenderId,

            // Pré-seleciona os IDs das categorias, cores e tamanhos atuais
            SelectedCategories = product.ProductCategories
                .Select(pc => pc.CategoryId)
                .ToList(),
            SelectedColors = product.ProductColors
                .Select(pc => pc.ColorId)
                .ToList(),
            SelectedSizes = product.ProductSizes
                .Select(ps => ps.SizeId)
                .ToList(),

            // Carrega as imagens que já existem
            ExistingImages = product.ProductImages
                .OrderBy(pi => pi.ImageOrder)
                .ToList(),

            Stock = product.StockProducts
                .Select(
                    s => new StockEditViewModel {
                        StockId = s.StockId,
                        ProductId = s.ProductId,
                        ColorId = s.ColorId,
                        SizeId = s.SizeId,
                        StockQuantity = s.StockQuantity,
                        // Preenche os dados auxiliares que o JS vai usar
                        ColorName = s.Color?.Color, // Adiciona '?' para segurança
                        HexColor = s.Color?.HexColor,
                        SizeName = s.Size?.Size
                    }
                )
                .ToList(),

            // Carrega todas as opções disponíveis para os dropdowns/checkboxes
            AvailableGenders = _genderRepository.FindAll(),
            AvailableCategories = _categoryRepository.FindAll(),
            AvailableColors = _colorRepository.FindAll(),
            AvailableSizes = _sizeRepository.FindAll()
        };

        return View(viewModel);
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Edit(ProductEditViewModel viewModel) {
        Console.WriteLine($"---> PROCESSO DE EDIÇÃO INICIADO para o produto ID: {viewModel.ProductId}");

        // Inicializa listas para evitar exceções de nulo
        if (viewModel.SelectedCategories == null) viewModel.SelectedCategories = new List<int>();
        if (viewModel.SelectedColors == null) viewModel.SelectedColors = new List<int>();
        if (viewModel.SelectedSizes == null) viewModel.SelectedSizes = new List<int>();
        if (viewModel.Stock == null) viewModel.Stock = new List<StockEditViewModel>();
        if (viewModel.ImagesToDelete == null) viewModel.ImagesToDelete = new List<int>();
        if (viewModel.NewImages == null) viewModel.NewImages = new List<IFormFile>();

        if (ModelState.IsValid) {
            Console.WriteLine("-> ModelState é válido. Iniciando o salvamento...");
            try {
                var productToUpdate = _productRepository.FindById(viewModel.ProductId);
                if (productToUpdate == null) {
                    Console.WriteLine($"ERRO: Produto com ID {viewModel.ProductId} não encontrado.");
                    return NotFound();
                }

                // 1. Atualiza as propriedades simples
                productToUpdate.ProductName = viewModel.ProductName;
                productToUpdate.ProductDescription = viewModel.ProductDescription;
                productToUpdate.ProductPrice = viewModel.ProductPrice;
                productToUpdate.GenderId = viewModel.GenderId;

                // 2. Recalcula a quantidade total do estoque
                productToUpdate.ProductQuantity = viewModel.Stock.Sum(s => s.StockQuantity);
                Console.WriteLine($"-> Quantidade total de estoque calculada: {productToUpdate.ProductQuantity}");

                // 3. Atualiza as coleções
                _productRepository.UpdateProductCategories(productToUpdate.ProductId, viewModel.SelectedCategories);
                _productRepository.UpdateProductColors(productToUpdate.ProductId, viewModel.SelectedColors);
                _productRepository.UpdateProductSizes(productToUpdate.ProductId, viewModel.SelectedSizes);
                _productRepository.UpdateStock(productToUpdate.ProductId, viewModel.Stock);
                Console.WriteLine("-> Coleções (Categorias, Cores, Tamanhos, Estoque) preparadas para atualização.");

                // 4. Gerencia as imagens
                if (viewModel.ImagesToDelete.Any()) {
                    Console.WriteLine($"-> Deletando {viewModel.ImagesToDelete.Count} imagem(ns).");
                    _productRepository.DeleteImages(viewModel.ImagesToDelete, _environment.WebRootPath);
                }

                if (viewModel.NewImages.Any()) {
                    Console.WriteLine($"-> Adicionando {viewModel.NewImages.Count} nova(s) imagem(ns).");
                    string uploadDir = Path.Combine(_environment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    int lastOrder = productToUpdate.ProductImages.Any() ?
                        productToUpdate.ProductImages.Max(i => i.ImageOrder ?? 0) : 0;
                    foreach (var imageFile in viewModel.NewImages) {
                        if (imageFile.Length > 0) {
                            string uniqueFileName =
                                $"{Guid.NewGuid().ToString().Substring(0, 8)}-{Path.GetFileName(imageFile.FileName)}";
                            string filePath = Path.Combine(uploadDir, uniqueFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create)) {
                                await imageFile.CopyToAsync(stream);
                            }

                            _productRepository.CreateProductImage(
                                new ProductImageModel {
                                    ProductId = productToUpdate.ProductId,
                                    ProductImagePath = $"/images/products/{uniqueFileName}",
                                    ImageOrder = ++lastOrder
                                }
                            );
                        }
                    }
                }

                // 5. Salva todas as alterações no banco de dados
                Console.WriteLine("-> Chamando SaveChanges() para persistir tudo no banco de dados...");
                await _productRepository.SaveChanges();
                Console.WriteLine($"SUCESSO: Produto ID {productToUpdate.ProductId} atualizado no banco.");

                TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                // Retorna uma resposta de sucesso para o AJAX
                return Ok(
                    new {
                        success = true,
                        redirectToUrl = Url.Action(nameof(Index))
                    }
                );
            } catch (Exception ex) {
                Console.WriteLine($"--- ERRO CATCH: Ocorreu uma exceção ao salvar o produto {viewModel.ProductId} ---");
                Console.WriteLine(ex.ToString()); // Imprime a exceção completa no console
                return StatusCode(
                    500,
                    new {
                        success = false,
                        message = "Ocorreu um erro inesperado no servidor. Veja o console da aplicação para detalhes."
                    }
                );
            }
        }

        // Se o ModelState for inválido
        Console.WriteLine("AVISO: ModelState é inválido. Retornando erros de validação.");
        var errors = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value
                .Errors
                .Select(e => e.ErrorMessage)
                .ToArray()
        );
        // Retorna os erros de validação para o AJAX
        return BadRequest(
            new {
                success = false,
                errors = errors
            }
        );
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
                        kvp => kvp.Value
                            .Errors
                            .Select(e => e.ErrorMessage)
                            .ToArray()
                    );

                    return BadRequest(
                        new {
                            sucess = false,
                            errors = errors,
                            message = "Dados Invalidos"
                        }
                    );
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

    // No ProductController.cs
// Não se esqueça de injetar IWebHostEnvironment no construtor se ainda não o fez.

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Kill(int id) {
        var productToDelete = _productRepository.FindById(id);
        if (productToDelete == null) {
            return NotFound(
                new {
                    success = false,
                    message = "Produto não encontrado."
                }
            );
        }

        try {
            // 1. Deleta os arquivos de imagem físicos do servidor
            foreach (var image in productToDelete.ProductImages) {
                if (!string.IsNullOrEmpty(image.ProductImagePath)) {
                    var fullPath = Path.Combine(_environment.WebRootPath, image.ProductImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath)) {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            // 2. Deleta o produto e todos os seus registros relacionados do banco
            _productRepository.Kill(id);
            await _productRepository.SaveChanges();

            return Ok(
                new {
                    success = true,
                    message = "Produto excluído com sucesso!"
                }
            );
        } catch (Exception ex) {
            // Em caso de erro (ex: o produto está em um pedido e não pode ser excluído)
            return StatusCode(
                500,
                new {
                    success = false,
                    message =
                        "Ocorreu um erro ao excluir o produto. Verifique se ele não está associado a pedidos existentes."
                }
            );
        }
    }
}