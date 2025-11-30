using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<BannerModel> 
{
    public void Configure(EntityTypeBuilder<BannerModel> builder) {
        builder.ToTable("Banners");

        builder.HasKey(b => b.BannerId);

        builder
               .Property(b => b.BannerId)
               .HasColumnName("banner_id")
               .ValueGeneratedOnAdd();

        builder
               .Property(b => b.Title)
               .HasColumnName("banner_title")
               .IsRequired()
               .HasMaxLength(100);

        builder
               .Property(b => b.Description)
               .HasColumnName("banner_description")
               .HasMaxLength(255)
               .IsRequired(false);

        builder
               .Property(b => b.ImageUrl)
               .HasColumnName("banner_path")
               .IsRequired()
               .HasMaxLength(500);

        builder
               .Property(b => b.LinkUrl)
               .HasColumnName("banner_link_url")
               .HasMaxLength(500)
               .IsRequired(false);

        builder
               .Property(b => b.IsActive)
               .HasColumnName("banner_is_active")
               .IsRequired()
               .HasDefaultValue(true);
    }
}