using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.Services;

public interface IFileService
{

    Task<string> UploadFile(IFormFile file, string folder);
    Task<bool> DeleteFile(string filePath);
}