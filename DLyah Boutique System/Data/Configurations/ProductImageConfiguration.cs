using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageModel> {
    public void Configure( EntityTypeBuilder<ProductImageModel> builder ) {
        builder.ToTable( "ProductImages" );

        builder.HasKey( pi => pi.ProductImageId );
        
        builder
            .Property( pi => pi.ProductImageId )
            .HasColumnName( "product_image_id" );
        
        builder
            .Property( pi => pi.ProductId )
            .HasColumnName( "product_id" );

        builder
            .Property( pi => pi.ProductImagePath )
            .HasColumnName( "product_image_path" )
            .IsRequired()
            .HasMaxLength( 255 );

        builder
            .Property( pi => pi.ImageOrder )
            .HasColumnName( "image_order" );

        builder
            .HasOne( pi => pi.Product )
            .WithMany( p => p.ProductImages )
            .HasForeignKey( pi => pi.ProductId )
            .HasConstraintName( "FK_ProductImage_Product" )
            .OnDelete(DeleteBehavior.Cascade);
    }
}