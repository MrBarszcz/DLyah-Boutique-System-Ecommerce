using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class OrdemItemConfiguration : IEntityTypeConfiguration<OrderItemModel> {
    public void Configure( EntityTypeBuilder<OrderItemModel> builder ) {
        builder.ToTable( "OrderItems" );

        builder.HasKey( oi => new {
                oi.OrderId,
                oi.ProductId
            }
        );
        
        builder
            .Property( oi => oi.OrderId )
            .HasColumnName( "order_id" )
            .IsRequired();
        
        builder
            .Property( oi => oi.ProductId )
            .HasColumnName( "product_id" )
            .IsRequired();
        
        builder
            .Property( oi => oi.OrderQuantity )
            .HasColumnName( "quantity" )
            .IsRequired();

        builder
            .Property( oi => oi.OrderPriceUnitary )
            .HasColumnName( "price_per_unit" )
            .IsRequired()
            .HasColumnType( "DECIMAL(10, 2)" );

        builder
            .Property( oi => oi.Subtotal )
            .HasColumnName( "total_value" )
            .IsRequired()
            .HasColumnType( "DECIMAL(10, 2)" );

        builder
            .HasOne( oi => oi.Order )
            .WithMany( o => o.OrderItems )
            .HasForeignKey( oi => oi.OrderId )
            .HasConstraintName( "FK_OrderItem_Order" )
            .OnDelete( DeleteBehavior.NoAction );

        builder
            .HasOne( oi => oi.Product )
            .WithMany( p => p.OrderItems )
            .HasForeignKey( oi => oi.ProductId )
            .HasConstraintName( "FK_OrderItem_Product" )
            .OnDelete( DeleteBehavior.NoAction );
    }
}