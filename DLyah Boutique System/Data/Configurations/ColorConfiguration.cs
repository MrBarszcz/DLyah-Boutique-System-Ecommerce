using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class ColorConfiguration : IEntityTypeConfiguration<ColorModel> {
    public void Configure( EntityTypeBuilder<ColorModel> builder ) {
        builder.ToTable( "Colors" );
        
        builder.HasKey( c => c.ColorId );
        
        builder.Property( c => c.ColorId ).HasColumnName( "color_id" );
        
        builder.Property( c => c.Color ).HasColumnName( "color" ).IsRequired().HasMaxLength( 50 );
        
        builder.HasIndex( c => c.Color ).HasDatabaseName( "IX_Colors_color" ).IsUnique();
        // Explicitly name the index
        builder.Property( c => c.HexColor ).HasColumnName( "hex_color" ).IsRequired().HasMaxLength( 7 );
        
        builder.HasIndex( c => c.HexColor )
               .HasDatabaseName( "IX_Colors_hex_color" )
               .IsUnique();
    }
}