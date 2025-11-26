using System.Globalization;
using DLyah_Boutique_System.Extensions;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class CategoryController : Controller {
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController( ICategoryRepository categoryRepository, ILogger<CategoryController> logger ) {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    // GET
    public IActionResult Index() {
        // lists the categories and has a button that sends to Register, UpdateCategory, DeleteCategory
        List<CategoryModel> categories = _categoryRepository.FindAll();

        return View( categories );
    }

    public IActionResult Edit( int id ) {
        CategoryModel category = _categoryRepository.FindById( id );

        if ( category == null )
            return NotFound();

        var viewModel = new CategoryEditViewModel {
            CategoryId = category.CategoryId,
            Category = category.Category
        };

        return View( viewModel );
    }

    public IActionResult Register() {
        return View( new CategoryViewModel() );
    }

    // POST
    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public IActionResult Edit( CategoryEditViewModel viewModel ) {
        if ( !ModelState.IsValid ) {
            return View( viewModel );
        }

        try {
            // Verificar se o novo nome já existe E não é ele mesmo
            var categoryIsExists = _categoryRepository.FindByName( viewModel.Category );

            if ( categoryIsExists != null && categoryIsExists.CategoryId != viewModel.CategoryId ) {
                ModelState.AddModelError( "Category", "Essa categoria já está em uso" );

                return View( viewModel );
            }

            var editedCategory = new CategoryModel {
                CategoryId = viewModel.CategoryId,
                Category = viewModel.Category.ToTitleCase()
            };

            _categoryRepository.Update( editedCategory );

            TempData[ "SuccessMessage" ] = "Categoria atualizada com sucesso!";

            return RedirectToAction( "Index" );
        } catch ( Exception ex ) {
            TempData["ErrorMessage"] = "Falha ao atualizar a categoria. Tente novamente";
            _logger.LogError( ex, $"Erro ao atualizar a categoria {viewModel.CategoryId}" );

            return View( viewModel );
        }
    }

    [ HttpPost ]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register( CategoryViewModel viewModel ) {
        if ( !ModelState.IsValid ) {
            return View( viewModel ); // Retorna a view com a mensagem de erro
        }

        try {
            TextInfo textInfo = new CultureInfo( "pt-BR", false ).TextInfo;

            // Separo a string enviada a partir de pontos e vírgulas (";"), padronizo formatação para TitleCase ("String" envés de "string" ou "STRING"), não permito que repetições, transformo tudo em uma lista 
            var categoriesToSave = viewModel.Category
                                            .Split( ';',
                                                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                                            )
                                            .Select( s => s.ToTitleCase() )
                                            .Distinct( StringComparer.OrdinalIgnoreCase )
                                            .ToList();

            int countSuccesses = 0;
            int countExisting = 0;

            foreach ( var category in categoriesToSave ) {
                var categoryIsExists = _categoryRepository.FindByName( category );

                if ( categoryIsExists != null ) { // Caso já exista no banco de dados, pule pra próxima
                    countExisting++;
                    continue;
                }

                var newCategoryModel = new CategoryModel { Category = category };

                _categoryRepository.Add( newCategoryModel );
                countSuccesses++;
            }

            if ( countSuccesses != 0 ) { // Caso haja alguma categoria que não esteja no db ainda
                await _categoryRepository.SaveChanges();
                
                TempData[ "SuccessMessage" ] = $"{countSuccesses} categoria(s) cadastrada(s) com sucesso!";

                if ( countExisting > 0 ) {
                    TempData[ "WarningMessage" ] = $"{countExisting} categorias já existentes";
                }
                
            } else {
                TempData[ "WarningMessage" ] = "Nenhuma categoria nova foi adicionada (todas já existiam)";
            }

            return RedirectToAction( "Index" );
        } catch ( Exception ex ) {
            _logger.LogError( ex, "Erro ao salvar categorias em lote" );
            ModelState.AddModelError( "", "Ocorreu um erro inesperado ao salvar" );
        }

        return View( viewModel );
    }
}