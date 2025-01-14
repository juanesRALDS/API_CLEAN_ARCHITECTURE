using System.Net.Mail;
using SagaAserhi.Application.DTOs;

namespace SagaAserhi.Application.Interfaces.AttachmentUseCase;

public interface IUploadAttachmentUseCase
{
    Task<AttachmentDTO> ExecuteAsync(IFormFile file, string clientId, CancellationToken cancellationToken);
}