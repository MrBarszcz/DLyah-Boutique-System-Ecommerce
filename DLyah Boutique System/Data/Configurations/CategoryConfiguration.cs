using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryModel> {
    public void Configure( EntityTypeBuilder<CategoryModel> builder ) {
        builder.ToTable("Categories");
        
        builder.HasKey(c => c.CategoryId);
        
        builder
            .Property(c => c.CategoryId)
            .HasColumnName("category_id")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(c => c.Category)
            .HasColumnName("category")
            .IsRequired()
            .HasMaxLength(100);
    }
}