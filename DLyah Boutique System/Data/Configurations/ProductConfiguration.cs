using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductModel> {
    public void Configure( EntityTypeBuilder<ProductModel> builder ) {
        builder.ToTable("Products");

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductId)
            .HasColumnName("product_id");

        builder.Property(p => p.ProductName)
            .HasColumnName("product_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ProductDescription)
            .HasColumnName("product_description")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.ProductPrice)
            .HasColumnName("product_price")
            .IsRequired()
            .HasColumnType("DECIMAL(10, 2)");

        builder.Property(p => p.ProductQuantity)
            .HasColumnName("product_quantity");

        builder.Property(p => p.GenderId)
            .HasColumnName("gender_id")
            .IsRequired();

        builder.HasOne(p => p.Gender)
            .WithMany(g => g.Products)
            .HasForeignKey(p => p.GenderId)
            .HasConstraintName("FK_Product_Gender")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .HasConstraintName("FK_OrderItem_Product")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .HasConstraintName("FK_ProductCategory_Product");
        
        builder.HasMany(p => p.ProductColors)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .HasConstraintName("FK_ProductColor_Product");
        
        builder.HasMany(p => p.ProductSizes)
            .WithOne(ps => ps.Product)
            .HasForeignKey(ps => ps.ProductId)
            .HasConstraintName("FK_ProductSize_Product");

        builder.HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .HasConstraintName("FK_ProductImage_Product")
            .OnDelete(DeleteBehavior.Cascade);
    }
}