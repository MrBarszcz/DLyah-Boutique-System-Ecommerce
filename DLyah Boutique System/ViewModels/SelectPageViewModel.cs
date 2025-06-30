using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DLyah_Boutique_System.ViewModels;

public class SelectPageViewModel {
    [Required(ErrorMessage = "Por favor, selecione uma página para gerenciar.")]
    public string SelectedPageName { get; set; }

    public List<SelectListItem> AvailablePages { get; set; }
}