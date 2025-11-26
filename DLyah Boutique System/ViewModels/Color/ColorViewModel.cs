using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class ColorViewModel {
    [Required(ErrorMessage = "Campo obrigatório!")]
    public string Color { get; set; }
    
    public string HexColor { get; set; }
}