using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel> {
    public void Configure( EntityTypeBuilder<PaymentModel> builder ) {
        builder.ToTable( "Payments" );
        
        builder.HasKey( p => p.PaymentId );
        
        builder
            .Property( p => p.PaymentId )
            .HasColumnName( "payment_id" ).ValueGeneratedOnAdd();
        
        builder
            .Property( p => p.OrderId )
            .HasColumnName( "order_id" )
            .IsRequired();
        
        builder
            .Property( p => p.PaymentMethod )
            .HasColumnName( "payment_method" )
            .IsRequired()
            .HasMaxLength( 50 );

        builder.HasCheckConstraint( 
            "CK_PaymentMethod",
            "[payment_method] IN ('Dinheiro', 'PIX', 'Boleto', 'Cartão de Crédito')"
        );
        
        builder
            .Property( p => p.PaymentStatus )
            .HasColumnName( "payment_status" )
            .IsRequired()
            .HasMaxLength( 20 );

        builder.HasCheckConstraint( "CK_PaymentStatus",
            "[payment_status] IN ('Estornado', 'Recusado', 'Aprovado', 'Aguardando')"
        );

        builder
            .Property( p => p.PaymentValuePaid )
            .HasColumnName( "payment_value_paid" )
            .IsRequired()
            .HasColumnType( "DECIMAL(10, 2)" );
        
        builder
            .Property( p => p.PaymentDate )
            .HasColumnName( "payment_date" ).HasDefaultValueSql( "GETDATE()" );

        builder.HasOne( p => p.Order )
               .WithOne( o => o.Payment )
               .HasForeignKey<PaymentModel>( p => p.OrderId )
               .OnDelete( DeleteBehavior.Cascade );
    }
}