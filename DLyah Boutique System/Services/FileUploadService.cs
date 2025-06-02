using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DLyah_Boutique_System.Services;

public class FileUploadService : IFileUploadService {
    private readonly IWebHostEnvironment _environment;

    public FileUploadService(IWebHostEnvironment environment) {
        _environment = environment;
    }

    public async Task<string?> UploadFileAsync(IFormFile file, string productName) {
        if (file == null || file.Length == 0) {
            return null;
        }

        try {
            // Gerar nome do arquivo
            string fileName = $"{productName.Replace(" ", "-")}-{Guid.NewGuid().ToString().Substring(0, 8)}{Path.GetExtension(file.FileName)}";
            string uploadDir = Path.Combine(_environment.WebRootPath, "images", "products");
            string filePath = Path.Combine(uploadDir, fileName);

            // Criar diretório se não existir
            if (!Directory.Exists(uploadDir)) {
                Directory.CreateDirectory(uploadDir);
            }

            // Salvar o arquivo diretamente no servidor
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            // Retornar a URL relativa
            return $"/images/products/{fileName}";

        } catch (Exception ex) {
            Console.WriteLine($"Erro ao salvar arquivo {file.FileName}: {ex.Message}");
            return null;
        }
    }
}