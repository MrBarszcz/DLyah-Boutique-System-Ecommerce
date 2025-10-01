using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLyah_Boutique_System.Models;

[Table("BannersPlacements")]
public class BannerPlacementModel {
    [Key]
    [Column("banner_placement_id")]
    public int BannerPlacementId { get; set; }

    [Required]
    [Column("banner_placement_pagename")]
    public string PageName { get; set; }
    
    [Required]
    [Column("banner_placement_position")]
    public string Position { get; set; }

    [Required]
    [Column("banner_placement_order")]
    public int DisplayOrder { get; set; }

    [Required]
    [Column("banner_placement_is_active")]
    public bool IsActive { get; set; }

    // Chave Estrangeira
    [Required]
    [Column("banner_id")]
    public int BannerId { get; set; }
    
    [Required]
    [Column("display_type")]
    public string DisplayType { get; set; } = "Default"; // Ex: "Carousel", "Mosaic", "Default"

    [Column("layout_name")]
    public string? LayoutName { get; set; } // Ex: "Duplo Vertical", pode ser nulo


    // Propriedade de navegação para o banner correspondente
    [ForeignKey("BannerId")]
    public virtual BannerModel Banner { get; set; }
}