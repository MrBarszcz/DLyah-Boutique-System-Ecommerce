using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLyah_Boutique_System.Data.Configurations;

public class UserProfileImageConfiguration : IEntityTypeConfiguration<UserProfileImageModel> {
    public void Configure( EntityTypeBuilder<UserProfileImageModel> builder ) {
        builder.ToTable( "UserProfileImages" );
        
        builder.HasKey( upi => upi.UserImageId );
        
        builder
            .Property( upi => upi.UserImageId )
            .HasColumnName( "user_image_id" );
        
        builder
            .Property( upi => upi.UserId )
            .HasColumnName( "user_id" ).IsRequired();
        
        builder
            .Property( upi => upi.UserImagePath )
            .HasColumnName( "user_image_path" )
            .HasMaxLength( 255 );

        builder
            .HasOne( upi => upi.User )
            .WithOne( u => u.UserProfileImage )
            .HasForeignKey<UserProfileImageModel>( upi => upi.UserId )
            .HasConstraintName( "FK_UserProfileImage_User" );
    }
}