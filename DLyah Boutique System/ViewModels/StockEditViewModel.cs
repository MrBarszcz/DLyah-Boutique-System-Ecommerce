namespace DLyah_Boutique_System.ViewModels;

public class StockEditViewModel
{
    public int StockId { get; set; } // ID do registro de estoque existente (0 se for um novo)
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public int StockQuantity { get; set; }

    // Propriedades auxiliares para o JavaScript
    public string? ColorName { get; set; }
    public string? SizeName { get; set; }
    public string? HexColor { get; set; }
}