using System.ComponentModel.DataAnnotations;

namespace DLyah_Boutique_System.ViewModels;

public class BannerViewModel {
    public int BannerId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }
    
    public IFormFile? ImageUpload { get; set; }
        
    // Para exibir a imagem atual na página de edição
    public string? ExistingImageUrl { get; set; }
    
    [Url(ErrorMessage = "Se preenchido, o link deve ser uma URL válida.")] // [Url] só válida se houver um valor
    public string? LinkUrl { get; set; }

    public bool IsActive { get; set; } = true;
}