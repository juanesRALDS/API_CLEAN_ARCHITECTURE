using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface IAttachmentRepository
{

    Task<Attachment> UploadAsync(Attachment attachment, CancellationToken cancellationToken);

    Task<Attachment> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Attachment>> GetByClientIdAsync(string clientId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}