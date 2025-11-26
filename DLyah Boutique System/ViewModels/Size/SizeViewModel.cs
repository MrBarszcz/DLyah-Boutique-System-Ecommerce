using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class SizeViewModel {
    [ Required(ErrorMessage = "Campo obrigatório!") ]
    public string Size { get; set; }
}