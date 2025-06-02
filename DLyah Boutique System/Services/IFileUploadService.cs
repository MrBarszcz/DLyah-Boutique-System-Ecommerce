namespace DLyah_Boutique_System;

public interface IFileUploadService {
    Task<string?> UploadFileAsync(IFormFile file, string productName);
}