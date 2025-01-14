using SagaAserhi.Application.Interfaces.AttachmentUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace SagaAserhi.Application.UseCases.AttachmentUseCase
{
    public class UploadAttachmentUseCase : IUploadAttachmentUseCase
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public UploadAttachmentUseCase(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<AttachmentDTO> ExecuteAsync(IFormFile file, string clientId, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No se ha proporcionado ningún archivo");

            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("El ID del cliente es requerido");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var attachment = new Attachment
            {
                Id = Guid.NewGuid().ToString(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileData = memoryStream.ToArray(),
                UploadDate = DateTime.UtcNow,
                PotentialClientId = clientId
            };

            var savedAttachment = await _attachmentRepository.UploadAsync(attachment, cancellationToken);

            return new AttachmentDTO
            {
                Id = savedAttachment.Id,
                FileName = savedAttachment.FileName,
                ContentType = savedAttachment.ContentType,
                UploadDate = savedAttachment.UploadDate,
                PotentialClientId = savedAttachment.PotentialClientId
            };
        }
    }
}