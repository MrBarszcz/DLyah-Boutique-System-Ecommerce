using System.ComponentModel.DataAnnotations;
using DLyah_Boutique_System.Models;

namespace DLyah_Boutique_System.ViewModels;

public class ProductEditViewModel
{
    // ID do produto que está sendo editado (essencial!)
    public int ProductId { get; set; }

    [ Required(ErrorMessage = "O nome do produto é obrigatório.") ]
    public string ProductName { get; set; }

    [ Required(ErrorMessage = "A descrição do produto é obrigatória.") ]
    public string ProductDescription { get; set; }

    [ Required(ErrorMessage = "O preço do produto é obrigatório.") ]
    [ Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.") ]
    public decimal ProductPrice { get; set; }

    [ Required(ErrorMessage = "O gênero é obrigatório.") ]
    public int GenderId { get; set; }

    // --- Listas para gerenciar seleções ---
    public List<int> SelectedCategories { get; set; } = new List<int>();
    public List<int> SelectedColors { get; set; } = new List<int>();
    public List<int> SelectedSizes { get; set; } = new List<int>();

    // --- Listas para popular as opções no formulário ---
    public List<CategoryModel> AvailableCategories { get; set; } = new List<CategoryModel>();
    public List<ColorModel> AvailableColors { get; set; } = new List<ColorModel>();
    public List<SizeModel> AvailableSizes { get; set; } = new List<SizeModel>();
    public List<GenderModel> AvailableGenders { get; set; } = new List<GenderModel>();

    // --- Gerenciamento de Imagens ---
    // Lista de imagens que já existem no produto
    public List<ProductImageModel> ExistingImages { get; set; } = new List<ProductImageModel>();

    // Lista para receber novas imagens do upload
    public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();

    // Lista de IDs das imagens existentes que o usuário marcou para deletar
    public List<int> ImagesToDelete { get; set; } = new List<int>();

    public List<StockEditViewModel> Stock { get; set; } = new List<StockEditViewModel>();
}