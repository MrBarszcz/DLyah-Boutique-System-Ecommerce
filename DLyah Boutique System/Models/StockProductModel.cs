namespace DLyah_Boutique_System.Models;

public class StockProductModel {
    public int StockId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    
    public int StockQuantity { get; set; }
    
    public virtual ProductModel? Product { get; set; }
    public virtual ColorModel? Color { get; set; }
    public virtual SizeModel? Size { get; set; }
}