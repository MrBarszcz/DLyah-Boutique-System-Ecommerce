using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class BannerPlacementConfiguration : IEntityTypeConfiguration<BannerPlacementModel> {
    public void Configure( EntityTypeBuilder<BannerPlacementModel> builder ) {
        builder.ToTable( "BannersPlacements" );

        builder.HasKey( bp => bp.BannerPlacementId );

        builder
               .Property( bp => bp.BannerPlacementId )
               .HasColumnName( "banner_placement_id" )
               .ValueGeneratedOnAdd();

        builder
               .Property( bp => bp.PageName )
               .HasColumnName( "banner_placement_pagename" )
               .IsRequired()
               .HasMaxLength( 50 );

        builder
               .Property( bp => bp.DisplayOrder )
               .HasColumnName( "banner_placement_order" )
               .IsRequired()
               .HasDefaultValue( 0 );

        builder
               .Property( bp => bp.IsActive )
               .HasColumnName( "banner_placement_is_active" )
               .IsRequired()
               .HasDefaultValue( true );

        builder
               .Property( bp => bp.BannerId )
               .HasColumnName( "banner_id" )
               .IsRequired();

        builder
               .HasOne( placement => placement.Banner )
               .WithMany( banner => banner.Placements )
               .HasForeignKey( placement => placement.BannerId )
               .HasConstraintName( "FK_BannerPlacements_Banners" )
               .OnDelete( DeleteBehavior.Cascade );
    }
}