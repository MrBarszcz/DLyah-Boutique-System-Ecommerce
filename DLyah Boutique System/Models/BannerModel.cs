using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLyah_Boutique_System.Models;

[Table("Banners")]
public class BannerModel {
    [Key]
    [Column("banner_id")]
    public int BannerId { get; set; }

    [Required]
    [Column("banner_title")]
    public string Title { get; set; }

    [Column("banner_description")]
    public string? Description { get; set; }

    [Required]
    [Column("banner_path")]
    public string ImageUrl { get; set; }

    [Column("banner_link_url")]
    public string? LinkUrl { get; set; }

    [Required]
    [Column("banner_is_active")]
    public bool IsActive { get; set; }

    // Propriedade de navegação: um banner pode ter muitos posicionamentos
    public virtual ICollection<BannerPlacementModel> Placements { get; set; } = new List<BannerPlacementModel>();
}