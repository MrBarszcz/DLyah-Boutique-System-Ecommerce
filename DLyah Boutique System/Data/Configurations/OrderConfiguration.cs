using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderModel> {
    public void Configure( EntityTypeBuilder<OrderModel> builder ) {
        builder.ToTable( "Orders" );

        builder.HasKey( o => o.OrderId );

        builder
            .Property( o => o.OrderId )
            .HasColumnName( "order_id" )
            .ValueGeneratedOnAdd();

        builder
            .Property( o => o.DateOrder )
            .HasColumnName( "date_order" )
            .HasDefaultValueSql( "GETDATE()" );
        
        builder
            .Property( o => o.OrderStatus )
            .HasColumnName( "order_status" ).IsRequired().HasMaxLength( 20 );

        builder.HasCheckConstraint( "CK_OrderStatus",
            "[order_status] IN ('cancelado', 'enviado', 'pago', 'pendente')"
        );

        builder
            .Property( o => o.OrderValueTotal )
            .HasColumnName( "order_value_total" )
            .IsRequired()
            .HasColumnType( "DECIMAL(10, 2)" );

        builder
            .HasOne( o => o.Client )
            .WithMany( c => c.Orders )
            .HasForeignKey( o => o.ClientId )
            .HasConstraintName( "FK_Order_Client" )
            .OnDelete( DeleteBehavior.NoAction );

        builder
            .HasMany( o => o.OrderItems )
            .WithOne( oi => oi.Order )
            .HasForeignKey( oi => oi.OrderId )
            .HasConstraintName( "FK_OrderItem_Order" )
            .OnDelete( DeleteBehavior.NoAction );
    }
}