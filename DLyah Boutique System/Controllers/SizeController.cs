using System.Globalization;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class SizeController : Controller {
    private readonly ISizeRepository _sizeRepository;
    private readonly ILogger<SizeController> _logger;

    public SizeController( ISizeRepository sizeRepository, ILogger<SizeController> logger ) {
        _sizeRepository = sizeRepository;
        _logger = logger;
    }

    // GET
    public IActionResult Index() {
        List<SizeModel> sizes = _sizeRepository.FindAll();

        return View( sizes );
    }

    public IActionResult Edit( int id ) {
        SizeModel size = _sizeRepository.FindById( id );

        if ( size == null )
            return NotFound();

        var viewModel = new SizeEditViewModel {
            SizeId = size.SizeId,
            Size = size.Size
        };

        return View( viewModel );
    }

    public IActionResult Register() {
        return View( new SizeViewModel() );
    }

    // POST
    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public IActionResult Edit( SizeModel viewModel ) {
        if ( !ModelState.IsValid ) {
            return View( viewModel );
        }

        try {
            var sizeIsExists = _sizeRepository.FindByName( viewModel.Size );

            if ( sizeIsExists != null && sizeIsExists.SizeId != viewModel.SizeId ) {
                ModelState.AddModelError( "Size", "Esse tamanho já está cadastrado" );

                return View( viewModel );
            }

            var editedSize = new SizeModel {
                SizeId = viewModel.SizeId,
                Size = viewModel.Size.ToUpper()
            };

            _sizeRepository.Update( editedSize );

            TempData[ "SuccessMessage" ] = "Tamanho atualizado com sucesso";

            return RedirectToAction( "Index" );
        } catch ( Exception e ) {
            TempData[ "ErrorMessage" ] = "Falha ao atualizar o tamanho. Tente novamente";
            _logger.LogError( e, $"Erro ao atualizar o tamanho {viewModel.SizeId}" );

            return View( viewModel );
        }
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Register( SizeViewModel viewModel ) {
        if ( !ModelState.IsValid ) {
            return View( viewModel );
        }

        try {

            var sizesToSave = viewModel.Size
                                       .Split( ';',
                                           StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                                       )
                                       .Select( s => s.ToUpper() )
                                       .Distinct( StringComparer.OrdinalIgnoreCase )
                                       .ToList();

            int countSuccesses = 0;
            int countExisting = 0;

            foreach ( var size in sizesToSave ) {
                var sizeIsExists = _sizeRepository.FindByName( size );

                if ( sizeIsExists != null ) {
                    countExisting++;

                    continue;
                }

                var newSizeModel = new SizeModel { Size = size };

                _sizeRepository.Add( newSizeModel );
                countSuccesses++;
            }

            if ( countSuccesses != 0 ) {
                await _sizeRepository.SaveChanges();

                TempData[ "SuccessMessage" ] = $"{countSuccesses} tamanhos(s) cadastrado(s) com sucesso!";

                if ( countExisting > 0 ) {
                    TempData[ "WarningMessage" ] = $"{countExisting} tamanhos já existentes";
                }
            } else {
                TempData[ "WarningMessage" ] = "Nenhum tamanho novo foi adicionado (todos já existiam)";
            }

            return RedirectToAction( "Index" );
        } catch ( Exception e ) {
            _logger.LogError( e, "Erro ao salvar tamanhos em lote" );
            ModelState.AddModelError( "", "Ocorreu um erro inesperado ao salvar" );
        }
        
        return View( viewModel );
    }
}