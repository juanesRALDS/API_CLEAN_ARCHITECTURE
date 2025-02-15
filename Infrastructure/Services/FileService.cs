using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.Interfaces.Services;

namespace SagaAserhi.Infrastructure.Services;
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadFile(IFormFile file, string folder)
    {
        try
        {
            // Asegurar que wwwroot existe
            string? wwwrootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(wwwrootPath))
            {
                wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(wwwrootPath))
                {
                    Directory.CreateDirectory(wwwrootPath);
                }
            }

            // Crear carpeta de destino
            string? uploadPath = Path.Combine(wwwrootPath, folder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string? fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string? filePath = Path.Combine(uploadPath, fileName);

            using (FileStream? stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folder, fileName).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al subir archivo: {ex.Message}");
        }
    }

    public async Task<bool> DeleteFile(string filePath)
    {
        try
        {
            string? fullPath = Path.Combine(_environment.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}