using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class StockProductConfiguration : IEntityTypeConfiguration<StockProductModel> {
    public void Configure( EntityTypeBuilder<StockProductModel> builder ) {
        builder.ToTable( "StockProducts" );
        
        builder.HasKey( si => si.StockId );

        builder
               .Property( sp => sp.StockId )
               .HasColumnName( "stock_product_id" );

        builder
               .Property( si => si.ProductId )
               .HasColumnName( "product_id" );
        
        builder
               .HasOne( si => si.Product )
               .WithMany( p => p.StockProducts )
               .HasForeignKey( si => si.ProductId )
               .HasConstraintName( "FK_StockItem_Product" )
               .OnDelete( DeleteBehavior.Cascade );

        builder
               .Property( si => si.ColorId )
               .HasColumnName( "color_id" );
        
        builder
               .HasOne( si => si.Color )
               .WithMany( c => c.StockProducts )
               .HasForeignKey( si => si.ColorId )
               .HasConstraintName( "FK_StockItem_Color" )
               .OnDelete( DeleteBehavior.Cascade );

        builder
               .Property( si => si.SizeId )
               .HasColumnName( "size_id" );

        builder
               .HasOne( si => si.Size )
               .WithMany( s => s.StockProducts )
               .HasForeignKey( si => si.SizeId )
               .HasConstraintName( "FK_StockItem_Size" )
               .OnDelete( DeleteBehavior.Cascade );

        builder
               .Property( si => si.StockQuantity )
               .HasColumnName( "quantity_stock" );

        builder
               .HasIndex( si => new {
                       si.ProductId,
                       si.ColorId,
                       si.SizeId
                   }
               )
               .HasDatabaseName( "UQ_StockItem_ProductColorSize" )
               .IsUnique();
    }
}