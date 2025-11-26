using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class ColorEditViewModel {
    public int ColorId { get; set; }
    
    [Required(ErrorMessage = "Campo obrigatório!")]
    public string Color { get; set; }
    
    public string HexColor { get; set; }
}