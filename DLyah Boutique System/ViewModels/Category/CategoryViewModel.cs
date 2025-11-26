using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class CategoryViewModel {
    [ Required(ErrorMessage = "Campo obrigatório!") ]
    public string Category { get; set; }
}