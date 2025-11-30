using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<AddressModel> {
    public void Configure( EntityTypeBuilder<AddressModel> builder ) {
        builder.ToTable( "Address" );

        builder.HasKey( a => a.AddressId );

        builder
            .Property( a => a.AddressId )
            .HasColumnName( "address_id" )
            .ValueGeneratedOnAdd();
        
        builder.Property( a => a.UserId ).HasColumnName( "user_id" ).IsRequired();
        
        builder.Property( a => a.AddressNumber ).HasColumnName( "address_number" ).IsRequired().HasMaxLength( 10 );
        
        builder.Property( a => a.AddressComplement ).HasColumnName( "address_complement" ).HasMaxLength( 50 );
        
        builder.Property( a => a.AddressNeighborhood ).HasColumnName( "address_neighborhood" );
        
        builder.Property( a => a.AddressCity ).HasColumnName( "address_city" ).IsRequired().HasMaxLength( 100 );
        
        builder.Property( a => a.AddressState ).HasColumnName( "address_state" ).IsRequired().HasMaxLength( 50 );
        
        builder.Property( a => a.AddressCep ).HasColumnName( "address_cep" ).IsRequired().HasMaxLength( 20 );

        builder
            .Property( a => a.AddressType )
            .HasColumnName( "address_type" )
            .IsRequired()
            .HasMaxLength( 10 )
            .HasDefaultValue( "ENTREGA" )
            .HasAnnotation( "Relational:CheckConstraint", "[address_type] IN ('RETIRADA', 'ENTREGA')" );

        builder
            .HasOne( a => a.User )
            .WithMany( u => u.Addresses )
            .HasForeignKey( a => a.UserId )
            .HasConstraintName( "FK_Address_User" );
    }
}