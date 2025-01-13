using System.Net.Mail;

namespace SagaAserhi.Application.Interfaces.AttachmentUseCase;

public interface IUploadAttachmentUseCase
{
    Task<Attachment> ExecuteAsync(IFormFile file, string clientId, CancellationToken cancellationToken);
}