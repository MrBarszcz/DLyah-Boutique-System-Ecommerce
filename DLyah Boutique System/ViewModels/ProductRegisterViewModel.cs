using System.ComponentModel.DataAnnotations;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.ViewModels;

public class ProductRegisterViewModel
{
    [ Required(ErrorMessage = "O nome do produto é obrigatório.") ]
    [ StringLength(100, ErrorMessage = "O nome do produto pode ter no máximo 100 caracteres.") ]
    public string? ProductName { get; set; }

    [ Required(ErrorMessage = "A descrição curta do produto é obrigatória.") ]
    public string? ProductDescription { get; set; }

    [ Required(ErrorMessage = "O valor do produto é obrigatório.") ]
    [ Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.") ]
    [ DataType(DataType.Currency) ]
    public decimal ProductPrice { get; set; }

    [ Required(ErrorMessage = "O gênero do produto é obrigatório.") ]
    public int GenderId { get; set; } // Apenas o ID, não o objeto GenderModel

    public List<IFormFile>? PImages { get; set; } // Para upload de múltiplas imagens

    public List<int>? SelectedCategories { get; set; }
    public List<int>? SelectedColors { get; set; }
    public List<int>? SelectedSizes { get; set; }

    // Lista para receber os dados de estoque do formulário dinâmico
    // O nome 'Stock' corresponde ao prefixo usado no name="" dos inputs do JS (ex: "Stock[0].ColorId")
    public List<StockProductModel>? Stock { get; set; }

    // Propriedades para popular as opções na View (alternativa à ViewBag)
    public List<GenderModel>? AvailableGenders { get; set; }
    public List<CategoryModel>? AvailableCategories { get; set; }
    public List<ColorModel>? AvailableColors { get; set; }
    public List<SizeModel>? AvailableSizes { get; set; }

    public ProductRegisterViewModel() {
        PImages = new List<IFormFile>();
        SelectedCategories = new List<int>();
        SelectedColors = new List<int>();
        SelectedSizes = new List<int>();
        Stock = new List<StockProductModel>();

        AvailableGenders = new List<GenderModel>();
        AvailableCategories = new List<CategoryModel>();
        AvailableColors = new List<ColorModel>();
        AvailableSizes = new List<SizeModel>();
    }
}