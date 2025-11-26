using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class SizeEditViewModel {
    public int SizeId { get; set; }
    
    [ Required(ErrorMessage = "Campo obrigatório!") ]
    public string Size { get; set; }
}