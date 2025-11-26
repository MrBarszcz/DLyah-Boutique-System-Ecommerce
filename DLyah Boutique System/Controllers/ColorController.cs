using System.Globalization;
using DLyah_Boutique_System.Extensions;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class ColorController : Controller {
    private readonly IColorRepository _colorRepository;
    private readonly ILogger<ColorController> _logger;

    public ColorController( IColorRepository colorRepository, ILogger<ColorController> logger ) {
        _colorRepository = colorRepository;
        _logger = logger;
    }

    // GET
    public IActionResult Index() {
        List<ColorModel> colors = _colorRepository.FindAll();

        return View( colors );
    }

    public IActionResult Edit( int id ) {
        ColorModel color = _colorRepository.FindById( id );

        if ( color == null )
            return NotFound();

        var ViewModel = new ColorEditViewModel {
            ColorId = color.ColorId,
            Color = color.Color,
            HexColor = color.HexColor
        };

        return View( ViewModel );
    }

    public IActionResult Register() {
        return View( new ColorViewModel() );
    }

    // POST
    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public IActionResult Edit( ColorEditViewModel viewModel ) {
        if ( ModelState.IsValid ) {
            return View( viewModel );
        }

        try {
            var colorIsExists = _colorRepository.FindByName( viewModel.Color );

            if ( colorIsExists != null && colorIsExists.ColorId != viewModel.ColorId ) {
                ModelState.AddModelError( "Color", "Essa cor já está cadastrada" );

                return View( viewModel );
            }

            var editedColor = new ColorModel {
                ColorId = viewModel.ColorId,
                Color = viewModel.Color.ToTitleCase(),
                HexColor = viewModel.HexColor
            };

            _colorRepository.Update( editedColor );

            TempData[ "SuccessMessage" ] = "Cor atualizada com sucesso";

            return RedirectToAction( "Index" );
        } catch ( Exception e ) {
            TempData[ "ErrorMessage" ] = "Falha ao atualizar o tamanho. Tente novamente";
            _logger.LogError( e, $"Erro ao atualizar o tamanho {viewModel.ColorId}" );

            return View( viewModel );
        }
    }

    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Register( ColorViewModel viewModel ) {
        if ( !ModelState.IsValid ) {
            return View( viewModel );
        }

        try {
            var colorIsExists = _colorRepository.FindByName( viewModel.Color );

            if ( colorIsExists != null ) {
                ModelState.AddModelError( "Color", "Essa cor já está cadastrada" );

                return View( viewModel );
            }

            var newColorModel = new ColorModel {
                Color = viewModel.Color.ToTitleCase(),
                HexColor = viewModel.HexColor
            };

            _colorRepository.Create( newColorModel );
            
            TempData[ "SuccessMessage" ] = $"cor cadastrada com sucesso!";
                
            return RedirectToAction( "Index" );
        } catch ( Exception e ) {
            _logger.LogError( e, "Erro ao salvar cor" );
            ModelState.AddModelError( "", "Ocorreu um erro inesperado ao salvar" );
        }

        return View( viewModel );
    }
}