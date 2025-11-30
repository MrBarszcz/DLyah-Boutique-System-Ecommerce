using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientModel> {
    public void Configure( EntityTypeBuilder<ClientModel> builder ) {
        builder.ToTable( "Clients" );
        
        builder.HasKey( c => c.ClientId );
        
        builder
               .Property( c => c.ClientId )
               .HasColumnName( "client_id" );
        
        builder
               .Property( c => c.ClientCpf )
               .HasColumnName( "client_cpf" )
               .IsRequired()
               .HasMaxLength( 20 );

        builder
               .HasIndex( c => c.ClientCpf )
               .HasDatabaseName( "IX_Clients_client_cpf" )
               .IsUnique(); // Explicitly name the index

        builder
               .Property( c => c.ClientPhonenumber )
               .HasColumnName( "client_phonenumber" )
               .IsRequired()
               .HasMaxLength( 15 );
        
        builder
               .Property( c => c.ClientDateBirth )
               .HasColumnName( "client_date_birth" );

        builder
               .Property( c => c.ClientDateRegister )
               .HasColumnName( "client_date_register" )
               .HasDefaultValueSql( "GETDATE()" );

        builder
               .Property( c => c.ClientStatus )
               .HasColumnName( "client_status" )
               .IsRequired()
               .HasMaxLength( 20 )
               .HasDefaultValue( "ativo" );

        builder.ToTable( tb =>
               tb.HasCheckConstraint( "CK_ClientStatus", "[client_status] IN ('suspenso', 'inativo', 'ativo')" )
        );
        
        builder
               .Property( c => c.UserId )
               .HasColumnName( "user_id" )
               .IsRequired();

        // A configuração do relacionamento com Order é feita em OrderConfiguration.
    }
}