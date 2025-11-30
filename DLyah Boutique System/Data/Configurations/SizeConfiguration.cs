using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class SizeConfiguration : IEntityTypeConfiguration<SizeModel> {
    public void Configure( EntityTypeBuilder<SizeModel> builder ) {
        builder.ToTable( "Sizes" );
        
        builder.HasKey( s => s.SizeId );
        
        builder
            .Property( s => s.SizeId )
            .HasColumnName( "size_id" );
        
        builder
            .Property( s => s.Size )
            .HasColumnName( "size" )
            .IsRequired()
            .HasMaxLength( 10 );
        
        builder
            .HasIndex( s => s.Size )
            .HasDatabaseName( "IX_Sizes_size" )
            .IsUnique();
    }
}