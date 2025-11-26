using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class CategoryEditViewModel {
    public int CategoryId { get; set; }
    
    [ Required(ErrorMessage = "Campo obrigatório!") ]
    public string Category { get; set; }
}