namespace DLyah_Boutique_System.Models;

public class UserProfileImageModel {
    public int UserImageId { get; set; }
    public int UserId { get; set; }
    public string? UserImagePath { get; set; }
    public virtual UserModel User { get; set; } = null!;
}