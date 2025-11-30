using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class GenderConfiguration : IEntityTypeConfiguration<GenderModel> {
    public void Configure( EntityTypeBuilder<GenderModel> builder ) {
        builder.ToTable( "Gender" );
        
        builder.HasKey( g => g.GenderId );
        
        builder.Property( g => g.GenderId ).HasColumnName( "gender_id" );
        
        builder.Property( g => g.Gender ).HasColumnName( "gender" ).IsRequired().HasMaxLength( 50 );
        
        builder.HasIndex( g => g.Gender ).HasDatabaseName( "IX_Gender_gender" ).IsUnique();
        
        builder.HasMany( g => g.Products )
               .WithOne( p => p.Gender )
               .HasForeignKey( p => p.GenderId )
               .HasConstraintName( "FK_Product_Gender" );
    }
}