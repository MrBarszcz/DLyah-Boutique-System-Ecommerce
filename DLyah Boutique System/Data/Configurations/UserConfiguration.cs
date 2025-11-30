using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserModel> {
    public void Configure( EntityTypeBuilder<UserModel> builder ) {
        builder.ToTable( "AspNetUsers" );

        builder
               .Property( u => u.UserNameComplete )
               .HasColumnName( "user_name_complete" )
               .IsRequired()
               .HasMaxLength( 100 );
        
        builder
               // Mapeando a propriedade 'UserName' herdada de IdentityUser
               .Property( u => u.UserName )
               .HasColumnName( "username" );
        
        builder
               // Mapeando a propriedade 'Email' herdada de IdentityUser
               .Property( u => u.Email )
               .HasColumnName( "user_email" );

        // O índice único no email já é criado por padrão pelo IdentityDbContext.

        builder
               .Property( u => u.UserType )
               .HasColumnName( "user_type" )
               .IsRequired()
               .HasMaxLength( 20 );

        builder
               .Property( u => u.UserDateRegister )
               .HasColumnName( "user_date_register" )
               .HasDefaultValueSql( "GETDATE()" );

        builder
               .HasOne( u => u.Client )
               .WithOne( c => c.User )
               .HasForeignKey<ClientModel>( c => c.UserId )
               .HasConstraintName( "FK_Client_User" );

        builder
               .HasMany( u => u.Addresses )
               .WithOne( a => a.User )
               .HasForeignKey( a => a.UserId )
               .HasConstraintName( "FK_Addresses_User" );

        builder
               .HasOne<UserProfileImageModel>( u => u.UserProfileImage )
               .WithOne( upi => upi.User )
               .HasForeignKey<UserProfileImageModel>( upi => upi.UserId )
               .HasConstraintName( "FK_UserProfileImages_User" );
    }
}